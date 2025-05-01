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
public class ConcertsController : ControllerBase
{
	private readonly ILogger<ConcertsController> _logger;
    private readonly IConcertRepository _concertRepository;
	private readonly ITicketRepository _ticketRepository;

	public ConcertsController(ILogger<ConcertsController> logger,
        IConcertRepository concertRepository,
        ITicketRepository ticketRepository)
    {
        _logger = logger;
        _concertRepository = concertRepository;
		_ticketRepository = ticketRepository;
	}

    [HttpGet]
    public async Task<IEnumerable<Concert>> Get()
    {
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _concertRepository.GetAllConcertsAsync();
	}

	[HttpGet("{concertId}")]
	public async Task<Concert> Get(int concertId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		return await _concertRepository.GetConcertByIdAsync(concertId);
	}

	[HttpPost("{concertId}")]
    public async Task<IActionResult> Post(int concertId, [FromBody] ConcertWithoutId concert)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (concert == null)
		{
			_logger.LogInformation("Invalid null Concert Input on Post");
			return BadRequest("Concert cannot be null");
		}

		var existingConcert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (existingConcert != null)
		{
			_logger.LogInformation("Concert with ID {concertId} already exists", concertId);
			return Conflict("A concert with that ID already exists. Use Put to modify the concert.");
		}

		var newConcert = concert.AddId(concertId);
		await _concertRepository.AddConcertAsync(newConcert);
		_logger.LogInformation("Concert {concertId} created", concertId);
		return CreatedAtAction(nameof(Get), new { id = newConcert.Id }, newConcert);
	}

	[HttpPut("{concertId}")]
	public async Task<IActionResult> Put(int concertId, [FromBody] ConcertWithoutId concert)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (concert == null)
		{
			_logger.LogInformation("Invalid null Concert Input on Put");
			return BadRequest("Concert cannot be null");
		}

		var oldConcert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (oldConcert == null)
		{
			_logger.LogInformation("Concert {concertId} not found for update", concertId);
			return NotFound();
		}

		var newConcert = concert.AddId(concertId);
		await _concertRepository.UpdateConcertAsync(newConcert);
		_logger.LogInformation("Concert {concertId} updated", concertId);
		return NoContent();
	}

	[HttpDelete("{concertId}")]
	public async Task<IActionResult> Delete(int concertId)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);
		var concert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (concert == null)
		{
			_logger.LogInformation("Concert {concertId} not found for delete", concertId);
			return NotFound();
		}
		await _concertRepository.DeleteConcertAsync(concertId);
		_logger.LogInformation("Concert {concertId} deleted", concertId);
		return NoContent();
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

	[HttpPost("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Post(int concertId, int ticketTypeId, [FromBody] TicketTypeWithoutIds ticketType)
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

		var existingTicket = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (existingTicket != null)
		{
			_logger.LogInformation("Invalid attempt to add TicketType with duplicate Id on Post");
			return Conflict("A ticket type with that ID already exists. Use Put to modify the ticket type.");
		}

		TicketType newTicketType = ticketType.AddIds(concertId, ticketTypeId);
		await _concertRepository.AddTicketType(newTicketType);
		_logger.LogInformation("Ticket Type {concertId} {ticketTypeId} created", concertId, ticketTypeId);
		return CreatedAtAction(nameof(GetTicketType), new { concertId = concertId, ticketTypeId = ticketTypeId }, newTicketType);
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
