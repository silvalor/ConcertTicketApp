using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using Microsoft.AspNetCore.Mvc;
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
        return await _concertRepository.GetAllConcertsAsync();
	}

	[HttpGet("{concertId}")]
	public async Task<Concert> Get(int concertId)
	{
		return await _concertRepository.GetConcertByIdAsync(concertId);
	}

	[HttpPost("{concertId}")]
    public async Task<IActionResult> Post(int concertId, [FromBody] ConcertWithoutId concert)
	{
		if (concert == null)
		{
			return BadRequest("Concert cannot be null");
		}

		var existingConcert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (existingConcert != null)
		{
			return Conflict("A concert with that ID already exists. Use Put to modify the concert.");
		}

		var newConcert = concert.AddId(concertId);
		await _concertRepository.AddConcertAsync(newConcert);
		return CreatedAtAction(nameof(Get), new { id = newConcert.Id }, newConcert);
	}

	[HttpPut("{concertId}")]
	public async Task<IActionResult> Put(int concertId, [FromBody] ConcertWithoutId concert)
	{
		if (concert == null)
		{
			return BadRequest("Concert cannot be null");
		}

		var oldConcert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (oldConcert == null)
		{
			return NotFound();
		}

		var newConcert = concert.AddId(concertId);
		await _concertRepository.UpdateConcertAsync(newConcert);
		return NoContent();
	}

	[HttpDelete("{concertId}")]
	public async Task<IActionResult> Delete(int concertId)
	{
		var concert = await _concertRepository.GetConcertByIdAsync(concertId);
		if (concert == null)
		{
			return NotFound();
		}
		await _concertRepository.DeleteConcertAsync(concertId);
		return NoContent();
	}

	[HttpGet("{concertId}/total-capacity")]
	public async Task<int> GetTotalCapacityByConcertId(int concertId)
	{
		var ticketTypes = await _concertRepository.GetTicketTypesByConcertIdAsync(concertId);
		int capacity = 0;
		foreach (var ticketType in ticketTypes)
			capacity += ticketType.Capacity;
		return capacity;
	}

	[HttpGet("{concertId}/ticket-types")]
	public async Task<IEnumerable<TicketType>> GetTicketTypes(int concertId)
	{
		return await _concertRepository.GetTicketTypesByConcertIdAsync(concertId);
	}

	[HttpGet("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<TicketType> GetTicketType(int concertId, int ticketTypeId)
	{
		return await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
	}

	[HttpPost("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Post(int concertId, int ticketTypeId, [FromBody] TicketTypeWithoutIds ticketType)
	{
		if (ticketType == null)
		{
			return BadRequest("TicketTypeIn cannot be null");
		}

		if (ticketType.Price < 0)
		{
			return BadRequest("No negative prices.");
		}

		if (ticketType.Capacity < 0)
		{
			return BadRequest("No negative capacities.");
		}

		var existingTicket = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (existingTicket != null)
		{
			return Conflict("A ticket type with that ID already exists. Use Put to modify the ticket type.");
		}

		TicketType newTicketType = ticketType.AddIds(concertId, ticketTypeId);
		await _concertRepository.AddTicketType(newTicketType);
		return CreatedAtAction(nameof(GetTicketType), new { concertId = concertId, ticketTypeId = ticketTypeId }, newTicketType);
	}

	[HttpPut("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Put(
		int concertId,
		int ticketTypeId,
		[FromBody] TicketTypeWithoutIds ticketType)
	{
		if (ticketType == null)
		{
			return BadRequest("TicketType cannot be null");
		}

		if (ticketType.Price < 0)
		{
			return BadRequest("No negative prices.");
		}

		if (ticketType.Capacity < 0)
		{
			return BadRequest("No negative capacities.");
		}

		var oldTicketType = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (oldTicketType == null)
		{
			return NotFound();
		}

		TicketType newTicketType = ticketType.AddIds(concertId, ticketTypeId);
		await _concertRepository.UpdateTicketTypeAsync(newTicketType);
		return NoContent();
	}

	[HttpDelete("{concertId}/ticket-types/{ticketTypeId}")]
	public async Task<IActionResult> Delete(int concertId, int ticketTypeId)
	{
		var concert = await _concertRepository.GetTicketTypeByIdAsync(concertId, ticketTypeId);
		if (concert == null)
		{
			return NotFound();
		}
		await _concertRepository.DeleteTicketTypeAsync(concertId, ticketTypeId);
		return NoContent();
	}
}
