using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace ConcertTicketApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ConcertController : ControllerBase
{
	private readonly ILogger<ConcertController> _logger;
    private readonly IConcertRepository _concertRepository;
	private readonly ITicketRepository _ticketRepository;

	public ConcertController(ILogger<ConcertController> logger,
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
}
