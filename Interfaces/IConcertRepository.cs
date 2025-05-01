using ConcertTicketApp.Models;

namespace ConcertTicketApp.Interfaces
{
	public interface IConcertRepository
	{
		Task<IEnumerable<Concert>> GetAllConcertsAsync();
		Task<Concert> GetConcertByIdAsync(int concertId);
		Task<Concert> AddConcertAsync(ConcertWithoutId concert);
		Task UpdateConcertAsync(Concert concert);
		Task DeleteConcertAsync(int concertId);
		Task<IEnumerable<TicketType>> GetTicketTypesByConcertIdAsync(int concertId);
		Task<TicketType> GetTicketTypeByIdAsync(int concertId, int ticketTypeId);
		Task<TicketType> AddTicketType(TicketTypeWithoutIds ticketType, int concertId);
		Task UpdateTicketTypeAsync(TicketType newTicketType);
		Task DeleteTicketTypeAsync(int concertId, int ticketTypeId);
	}
}
