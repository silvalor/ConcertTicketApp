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
	}
}
