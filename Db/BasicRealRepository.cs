using ConcertTicketApp.Interfaces;
using ConcertTicketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketApp.Db
{
	public class BasicRealRepository : IConcertRepository, ITicketRepository
	{
		private readonly ConcertDbContext _context;

		public BasicRealRepository(ConcertDbContext context)
		{
			_context = context;
		}

		public async Task<Concert> AddConcertAsync(ConcertWithoutId concert)
		{
			var newConcert = concert.AddId(0);
			_context.Concerts.Add(newConcert);
			await _context.SaveChangesAsync();
			return newConcert;
		}

		public Task<AddTicketResult> AddTicketAsync(TicketWithoutId ticket)
		{
			throw new NotImplementedException();
		}

		public Task<TicketType> AddTicketType(TicketTypeWithoutIds ticketType, int concertId)
		{
			throw new NotImplementedException();
		}

		public Task DeleteConcertAsync(int concertId)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTicketAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task DeleteTicketTypeAsync(int concertId, int ticketTypeId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Concert>> GetAllConcertsAsync()
		{
			return await _context.Concerts.ToListAsync();
		}

		public Task<IEnumerable<Ticket>> GetAllTicketsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<int> GetAvailableTicketCount(int concertId, int ticketTypeId)
		{
			throw new NotImplementedException();
		}

		public Task<Concert> GetConcertByIdAsync(int concertId)
		{
			throw new NotImplementedException();
		}

		public Task<Ticket> GetTicketByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Ticket>> GetTicketsByTicketTypeAsync(int ticketTypeId)
		{
			throw new NotImplementedException();
		}

		public Task<TicketType> GetTicketTypeByIdAsync(int concertId, int ticketTypeId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TicketType>> GetTicketTypesByConcertIdAsync(int concertId)
		{
			throw new NotImplementedException();
		}

		public Task UpdateConcertAsync(Concert concert)
		{
			throw new NotImplementedException();
		}

		public Task UpdateTicketAsync(Ticket ticket)
		{
			throw new NotImplementedException();
		}

		public Task UpdateTicketTypeAsync(TicketType newTicketType)
		{
			throw new NotImplementedException();
		}
	}
}
