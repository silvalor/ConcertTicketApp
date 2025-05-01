using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;

namespace ConcertTicketApp.Db
{
	public class TestRepository : IConcertRepository, ITicketRepository
	{
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

		private List<Ticket> _tickets = new List<Ticket>
		{
			new Ticket
			{
				Id = 1,
				TicketTypeId = 1,
				CustomerId = 1,
				PurchaseDate = DateTime.Now,
				Quantity = 2,
				PurchaseTotal = 200.00m,
				Status = "Purchased"
			},
			new Ticket
			{
				Id = 2,
				TicketTypeId = 2,
				CustomerId = 2,
				PurchaseDate = DateTime.Now,
				Quantity = 1,
				PurchaseTotal = 50.00m,
				Status = "Reserved"
			}
		};

		public async Task AddConcertAsync(Concert concert)
		{
			_concerts.Add(concert);
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

		public async Task AddTicketAsync(Ticket ticket)
		{
			_tickets.Add(ticket);
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

		public async Task AddTicketType(TicketType ticketType)
		{
			_ticketTypes.Add(ticketType);
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
