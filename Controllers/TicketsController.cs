using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.Design;

namespace ConcertTicketApp.Controllers;

[ApiController]
[Route("v{version:apiVersion}/tickets")]
[ApiVersion("1.0")]
public class TicketsController : ControllerBase
{
	private readonly ILogger<TicketsController> _logger;
    private readonly IConcertRepository _concertRepository;
	private readonly ITicketRepository _ticketRepository;
	private readonly IPaymentProcessingService _paymentProcessor;
	private readonly IIdempotencyCache _idempotencyCache;

	public TicketsController(ILogger<TicketsController> logger,
        IConcertRepository concertRepository,
        ITicketRepository ticketRepository,
		IPaymentProcessingService paymentProcessor,
		IIdempotencyCache idempotencyCache)
    {
        _logger = logger;
        _concertRepository = concertRepository;
		_ticketRepository = ticketRepository;
		_paymentProcessor = paymentProcessor;
		_idempotencyCache = idempotencyCache;
	}

	[HttpGet("{ticketId}")]
	public async Task<Ticket> GetTicketsById(int ticketId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _ticketRepository.GetTicketByIdAsync(ticketId);
	}

	[HttpPost("reservations")]
	public async Task<IActionResult> Post([FromBody] TicketReservation ticketReservation)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (ticketReservation == null)
		{
			_logger.LogInformation("Invalid null Ticket Reservation Input on Post");
			return BadRequest("Ticket Reservation cannot be null");
		}

		TicketType ticketType = await _concertRepository.GetTicketTypeByIdAsync(
			ticketReservation.ConcertId, ticketReservation.TicketTypeId);
		if (ticketType == null)
		{
			_logger.LogInformation("No ticket type found with concertId {ConcertId} and TicketTypeId {TicketTypeId}", ticketReservation.ConcertId, ticketReservation.TicketTypeId);
			return BadRequest($"No ticket type found with concertId {ticketReservation.ConcertId} and TicketTypeId {ticketReservation.TicketTypeId}");
		}

		AddTicketResult newTicketResult = await _ticketRepository.AddTicketAsync(ticketReservation.ToTicketWithoutId(DateTime.Now));
		if(newTicketResult.Status == AddTicketResult.AddTicketStatus.InsufficientTickets)
		{
			_logger.LogInformation("Insufficient tickets available for Ticket Reservation");
			return BadRequest("Insufficient tickets available");
		}

		if(newTicketResult.NewTicket == null)
		{
			_logger.LogError("Failed to create new ticket reservation.");
			return StatusCode(500, "Failed to create new ticket reservation.");
		}

		var newTicket = newTicketResult.NewTicket;

		_logger.LogInformation("Concert {concertId} created", newTicket.Id);
		return CreatedAtAction(nameof(GetTicketsById), new { ticketId = newTicket.Id }, newTicket);
	}

	[HttpPatch("{ticketId}")]
	public async Task<IActionResult> ConvertToPurchase(
	int ticketId,
	[FromBody] ConvertHoldToPurchaseRequest request,
	[FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
	{
		if (string.IsNullOrEmpty(idempotencyKey))
			return BadRequest("Idempotency-Key header is required");

		var idempotencyResult = await _idempotencyCache.TryGet<IActionResult>(idempotencyKey);
		if (idempotencyResult.IsSuccess)
		{
			return idempotencyResult.Response;
		}

		if (request.Operation != "convert-to-purchase")
		{
			_logger.LogInformation("Invalid operation: {operation}", request.Operation);
			return await _idempotencyCache.AddAndReturn<IActionResult>(
				idempotencyKey,
				BadRequest("Invalid operation"), 
				DateTime.Now.AddMinutes(5));
		}

		var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
		if (ticket == null)
		{ 
			_logger.LogInformation("Ticket not found");
			return await _idempotencyCache.AddAndReturn<IActionResult>(
				idempotencyKey,
				NotFound(),
				DateTime.Now.AddMinutes(5));
		}

		if(ticket.Status != "Reserved")
		{
			_logger.LogInformation("Ticket is not in a reserved state");
			return await _idempotencyCache.AddAndReturn<IActionResult>(
				idempotencyKey,
				BadRequest("Ticket is not in a reserved state"),
				DateTime.Now.AddMinutes(5));
		}

		if (ticket.ReserveExpiration.HasValue && ticket.ReserveExpiration < DateTime.Now)
		{
			_logger.LogInformation("Ticket reservation has expired");
			return await _idempotencyCache.AddAndReturn<IActionResult>(
				idempotencyKey,
				BadRequest("Ticket reservation has expired"),
				DateTime.Now.AddMinutes(5));
		}

		var paymentResult = await _paymentProcessor.ProcessPaymentAsync(request.PaymentToken);
		if(!paymentResult.IsSuccess)
		{
			_logger.LogInformation("Payment processing failed: {error}", paymentResult.ErrorMessage);
			return await _idempotencyCache.AddAndReturn<IActionResult>(
				idempotencyKey,
				BadRequest(paymentResult.ErrorMessage),
				DateTime.Now.AddMinutes(5));
		}

		Ticket purchasedTicket = ticket.CopyWith(status: "Purchased", purchaseDate: DateTime.Now);
		await _ticketRepository.UpdateTicketAsync(purchasedTicket);

		return await _idempotencyCache.AddAndReturn<IActionResult>(
			idempotencyKey,
			Ok(paymentResult.PurchaseReceipt),
			DateTime.Now.AddMinutes(5));
	}

	[HttpDelete("{ticketId}/reservations")]
	public async Task<IActionResult> CancelReservation(
	int ticketId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		Ticket ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);

		if (ticket == null)
		{
			_logger.LogInformation("No ticket found for {ticketId}", ticketId);
			return BadRequest($"No ticket found for {ticketId}");
		}

		if (ticket.Status != "Reserved")
		{
			_logger.LogInformation("Ticket {ticketId} is not reserved", ticketId);
			return BadRequest($"Ticket {ticketId} is not reserved");
		}

		await _ticketRepository.UpdateTicketAsync(ticket.CopyWith(status: "Cancelled"));
		return Ok();
	}
}
