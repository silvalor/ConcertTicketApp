using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;

namespace ConcertTicketApp.Db
{
	public class TestRepository : IConcertRepository, ITicketRepository
	{
		private static List<Concert> _concerts = new List<Concert>
		{
			new Concert
			{
				Id = 1,
				Name = "Concert 1",
				Date = DateTime.Now.AddDays(1),
				Venue = "Venue 1",
				Description = "Description 1",
			},
			new Concert
			{
				Id = 2,
				Name = "Concert 2",
				Date = DateTime.Now.AddDays(2),
				Venue = "Venue 2",
				Description = "Description 2",
			},
			new Concert
			{
				Id = 3,
				Name = "Concert 3",
				Date = DateTime.Now.AddDays(3),
				Venue = "Venue 3",
				Description = "Description 3",
			}
		};

		private static List<TicketType> _ticketTypes = new List<TicketType>
		{
			new TicketType
			{
				ID = 1,
				Name = "VIP",
				ConcertId = 1,
				Price = 100.00m,
				Available = 50
			},
			new TicketType
			{
				ID = 2,
				Name = "Regular",
				ConcertId = 1,
				Price = 50.00m,
				Available = 100
			},
			new TicketType
			{
				ID = 3,
				Name = "Regular",
				ConcertId = 2,
				Price = 70.00m,
				Available = 50
			},
			new TicketType
			{
				ID = 4,
				Name = "Regular",
				ConcertId = 3,
				Price = 80.00m,
				Available = 100
			}
		};

		private static List<Ticket> _tickets = new List<Ticket>
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
	}
}
