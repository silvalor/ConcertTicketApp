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

	public TicketsController(ILogger<TicketsController> logger,
        IConcertRepository concertRepository,
        ITicketRepository ticketRepository)
    {
        _logger = logger;
        _concertRepository = concertRepository;
		_ticketRepository = ticketRepository;
	}

	[HttpGet("{ticketId}")]
	public async Task<Ticket> GetTicketsById(int ticketId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _ticketRepository.GetTicketByIdAsync(ticketId);
	}

	//[HttpPost("reserved-tickets")]
	//public async Task<IActionResult> Post([FromBody] TicketReservation ticketReservation)
	//{
	//	_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

	//	if (ticketReservation == null)
	//	{
	//		_logger.LogInformation("Invalid null Ticket Reservation Input on Post");
	//		return BadRequest("Ticket Reservation cannot be null");
	//	}

	//	Ticket newTicket = await _ticketRepository.AddTicketAsync(ticketReservation.ToTicketWithoutId());
	//	_logger.LogInformation("Concert {concertId} created", newTicket.Id);
	//	return CreatedAtAction(nameof(GetTicketsById), new { ticketId = newTicket.Id }, newTicket);
	//}
}
