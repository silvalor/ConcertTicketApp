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
}
