using ConcertTicketApp.Models;

namespace ConcertTicketApp.Interfaces
{
	public interface IConcertRepository
	{
		Task<IEnumerable<Concert>> GetAllConcertsAsync();
		Task<Concert> GetConcertByIdAsync(int id);
		Task AddConcertAsync(Concert concert);
		Task UpdateConcertAsync(Concert concert);
		Task DeleteConcertAsync(int id);
		Task<IEnumerable<TicketType>> GetTicketTypesByConcertIdAsync(int concertId);
		Task<TicketType> GetTicketTypeByIdAsync(int concertId, int id);
		Task AddTicketType(TicketType ticketType);
		Task UpdateTicketTypeAsync(TicketType newTicketType);
		Task DeleteTicketTypeAsync(int concertId, int ticketTypeId);
	}
}
