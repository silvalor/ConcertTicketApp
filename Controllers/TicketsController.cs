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

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] ConcertWithoutId concert)
	{
		_logger.LogInformation("Starting request: {Method} {Path}", HttpContext.Request.Method, HttpContext.Request.Path);

		if (concert == null)
		{
			_logger.LogInformation("Invalid null Concert Input on Post");
			return BadRequest("Concert cannot be null");
		}

		Concert newConcert = await _concertRepository.AddConcertAsync(concert);
		_logger.LogInformation("Concert {concertId} created", newConcert.Id);
		return CreatedAtAction(nameof(Get), new { concertId = newConcert.Id }, newConcert);
	}
}
