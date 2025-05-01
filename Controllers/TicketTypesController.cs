using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.Design;

namespace ConcertTicketApp.Controllers;

[ApiController]
[Route("v{version:apiVersion}/concerts")]
[ApiVersion("1.0")]
public class TicketTypesController : ControllerBase
{
	private readonly ILogger<ConcertsController> _logger;
    private readonly IConcertRepository _concertRepository;

	public TicketTypesController(ILogger<ConcertsController> logger,
        IConcertRepository concertRepository)
    {
        _logger = logger;
        _concertRepository = concertRepository;
	}

	[HttpGet("{concertId}/total-capacity")]
	public async Task<int> GetTotalCapacityByConcertId(int concertId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		var ticketTypes = await _concertRepository.GetTicketTypesByConcertIdAsync(concertId);
		int capacity = 0;
		foreach (var ticketType in ticketTypes)
			capacity += ticketType.Capacity;

		_logger.LogInformation("Capacity: {capacity}", capacity);
		return capacity;
	}

	[HttpGet("{concertId}/ticket-types")]
	public async Task<IEnumerable<TicketType>> GetTicketTypes(int concertId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _concertRepository.GetTicketTypesByConcertIdAsync(concertId);
	}

	[HttpGet("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<TicketType> GetTicketType(int concertId, int ticketTypeId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
	}

	[HttpPost("{concertId}/ticket-types")]
	public async Task<IActionResult> Post(int concertId, [FromBody] TicketTypeWithoutIds ticketType)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (ticketType == null)
		{
			_logger.LogInformation("Invalid null TicketType Input on Post");
			return BadRequest("TicketTypeIn cannot be null");
		}

		if (ticketType.Price < 0)
		{
			_logger.LogInformation("Invalid TicketType with negative price on Post");
			return BadRequest("No negative prices.");
		}

		if (ticketType.Capacity < 0)
		{
			_logger.LogInformation("Invalid TicketType with negative capacity on Post");
			return BadRequest("No negative capacities.");
		}

		var newTicketType = await _concertRepository.AddTicketType(ticketType, concertId);
		_logger.LogInformation("Ticket Type {concertId} {ticketTypeId} created", concertId, newTicketType.Id);
		return CreatedAtAction(nameof(GetTicketType), new { concertId = concertId, ticketTypeId = newTicketType.Id }, newTicketType);
	}

	[HttpPut("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Put(
		int concertId,
		int ticketTypeId,
		[FromBody] TicketTypeWithoutIds ticketType)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (ticketType == null)
		{
			_logger.LogInformation("Invalid null TicketType Input on Put");
			return BadRequest("TicketType cannot be null");
		}

		if (ticketType.Price < 0)
		{
			_logger.LogInformation("Invalid TicketType with negative price on Put");
			return BadRequest("No negative prices.");
		}

		if (ticketType.Capacity < 0)
		{
			_logger.LogInformation("Invalid TicketType with negative capacity on Put");
			return BadRequest("No negative capacities.");
		}

		var oldTicketType = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (oldTicketType == null)
		{
			_logger.LogInformation("TicketType {concertId} {ticketTypeId} not found for update", concertId, ticketTypeId);
			return NotFound();
		}

		TicketType newTicketType = ticketType.AddIds(concertId, ticketTypeId);
		await _concertRepository.UpdateTicketTypeAsync(newTicketType);
		_logger.LogInformation("TicketType {concertId} {ticketTypeId} updated", concertId, ticketTypeId);
		return NoContent();
	}

	[HttpDelete("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Delete(int concertId, int ticketTypeId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		var concert = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (concert == null)
		{
			_logger.LogInformation("TicketType {concertId} {ticketTypeId} not found for delete", concertId, ticketTypeId);
			return NotFound();
		}
		await _concertRepository.DeleteTicketTypeAsync(concertId, ticketTypeId);
		_logger.LogInformation("TicketType {concertId} {ticketTypeId} deleted", concertId, ticketTypeId);
		return NoContent();
	}
}
