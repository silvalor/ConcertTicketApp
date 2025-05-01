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

	public ConcertsController(ILogger<ConcertsController> logger,
        IConcertRepository concertRepository)
    {
        _logger = logger;
        _concertRepository = concertRepository;
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
}
