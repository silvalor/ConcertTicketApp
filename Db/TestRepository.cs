using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using System;
using System.Net.Sockets;

namespace ConcertTicketApp.Db
{
	public class TestRepository : IConcertRepository, ITicketRepository
	{
		private int _nextConcertId = 4;
		private List<Concert> _concerts = new List<Concert>
		{
			new Concert
			{
				Id = 1,
				Name = "Concert 1",
				Date = DateTime.Parse("2025-05-01T05:23:10.901Z"),
				Venue = "Venue 1",
				Description = "Description 1",
			},
			new Concert
			{
				Id = 2,
				Name = "Concert 2",
				Date = DateTime.Parse("2025-05-02T05:23:10.901Z"),
				Venue = "Venue 2",
				Description = "Description 2",
			},
			new Concert
			{
				Id = 3,
				Name = "Concert 3",
				Date = DateTime.Parse("2025-05-03T05:23:10.901Z"),
				Venue = "Venue 3",
				Description = "Description 3",
			}
		};

		private int _nextTicketTypeId = 4;
		private List<TicketType> _ticketTypes = new List<TicketType>
		{
			new TicketType
			{
				Id = 1,
				ConcertId = 1,
				Name = "VIP",
				Price = 100.00m,
				Capacity = 50
			},
			new TicketType
			{
				Id = 2,
				ConcertId = 1,
				Name = "Regular",
				Price = 50.00m,
				Capacity = 100
			},
			new TicketType
			{
				Id = 3,
				ConcertId = 2,
				Name = "Regular",
				Price = 70.00m,
				Capacity = 50
			},
			new TicketType
			{
				Id = 4,
				ConcertId = 3,
				Name = "Regular",
				Price = 80.00m,
				Capacity = 100
			}
		};

		private int _nextTicketId = 3;
		private List<Ticket> _tickets = new List<Ticket>
		{
			new Ticket
			{
				Id = 1,
				ConcertId = 1,
				TicketTypeId = 1,
				CustomerId = 1,
				PurchaseDate = DateTime.Parse("2025-05-03T05:23:10.901Z"),
				Quantity = 2,
				Status = "Purchased"
			},
			new Ticket
			{
				Id = 2,
				ConcertId = 1,
				TicketTypeId = 2,
				CustomerId = 2,
				PurchaseDate = DateTime.Parse("2025-05-03T05:23:10.901Z"),
				Quantity = 1,
				Status = "Reserved"
			}
		};

		public async Task<Concert> AddConcertAsync(ConcertWithoutId concert)
		{
			var newConcert = concert.AddId(_nextConcertId);
			_nextConcertId++;
			_concerts.Add(newConcert);
			return newConcert;
		}

		public async Task DeleteConcertAsync(int id)
		{
			_concerts.RemoveAll(c => c.Id == id);
		}

		public async Task<IEnumerable<Concert>> GetAllConcertsAsync()
		{
			return _concerts;
		}

		public async Task<Concert> GetConcertByIdAsync(int id)
		{
			return _concerts.FirstOrDefault(c => c.Id == id);
		}

		public async Task UpdateConcertAsync(Concert concert)
		{
			_concerts.RemoveAll(c => c.Id == concert.Id);
			_concerts.Add(concert);
		}

		public async Task<AddTicketResult> AddTicketAsync(TicketWithoutId ticket)
		{
			int available = await GetAvailableTicketCount(ticket.ConcertId, ticket.TicketTypeId);
			if (available < ticket.Quantity)
			{
				return new AddTicketResult()
				{
					Status = AddTicketResult.AddTicketStatus.InsufficientTickets,
					NewTicket = null
				};
			}

			Ticket newTicket = ticket.ToTicket(_nextTicketId);
			_nextTicketId++;
			_tickets.Add(newTicket);
			return new AddTicketResult()
			{
				Status = AddTicketResult.AddTicketStatus.Success, NewTicket = newTicket
			};
		}

		public async Task<int> GetAvailableTicketCount(int concertId, int ticketTypeId)
		{
			var ticketType = _ticketTypes.FirstOrDefault(tt => tt.Id == ticketTypeId && tt.ConcertId == concertId);
			if (ticketType == null) return 0;
			int ticketsUnavailable = _tickets.Where(TicketIsConsumingAvailabilty).Sum(t => t.Quantity);
			return ticketType.Capacity - ticketsUnavailable;
		}

		private static bool TicketIsConsumingAvailabilty(Ticket ticket)
		{
			switch(ticket.Status)
			{
				case "Purchased":
					return true;
				case "Reserved":
					if (ticket.ReserveExpiration.HasValue)
						return ticket.ReserveExpiration > DateTime.Now;
					return false;
				default:
					return false;
			}
		}

		public async Task DeleteTicketAsync(int id)
		{
			_tickets.RemoveAll(t => t.Id == id);
		}
		public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
		{
			return _tickets;
		}
		public async Task<Ticket> GetTicketByIdAsync(int id)
		{
			return _tickets.FirstOrDefault(t => t.Id == id);
		}
		public async Task UpdateTicketAsync(Ticket ticket)
		{
			_tickets.RemoveAll(t => t.Id == ticket.Id);
			_tickets.Add(ticket);
		}

		public async Task<IEnumerable<TicketType>> GetTicketTypesByConcertIdAsync(int concertId)
		{
			return _ticketTypes.Where(tt => tt.ConcertId == concertId);
		}

		public async Task<IEnumerable<Ticket>> GetTicketsByTicketTypeAsync(int ticketTypeId)
		{
			return _tickets.Where(t => t.TicketTypeId == ticketTypeId);
		}

		public async Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId)
		{
			return _tickets.Where(t => t.CustomerId == customerId);
		}

		public async Task<TicketType> AddTicketType(TicketTypeWithoutIds ticketType, int concertId)
		{
			var newTicketType = ticketType.AddIds(concertId, _nextTicketTypeId);
			_nextConcertId++;
			_ticketTypes.Add(newTicketType);
			return newTicketType;
		}

		public async Task<TicketType> GetTicketTypeByIdAsync(int concertId, int id)
		{
			return _ticketTypes.Where(tt => tt.ConcertId == concertId && tt.Id == id).FirstOrDefault();
		}

		public async Task UpdateTicketTypeAsync(TicketType newTicketType)
		{
			_ticketTypes.RemoveAll(tt => tt.Id == newTicketType.Id);
			_ticketTypes.Add(newTicketType);
		}

		public async Task DeleteTicketTypeAsync(int concertId, int ticketTypeId)
		{
			_ticketTypes.RemoveAll(tt => tt.Id == ticketTypeId);
		}
	}
}
