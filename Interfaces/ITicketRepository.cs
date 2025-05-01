namespace ConcertTicketApp.Interfaces
{
	public interface ITicketRepository
	{
		Task AddTicketAsync(Ticket ticket);
		Task DeleteTicketAsync(int id);
		Task<IEnumerable<Ticket>> GetAllTicketsAsync();
		Task<Ticket> GetTicketByIdAsync(int id);
		Task UpdateTicketAsync(Ticket ticket);
		Task<IEnumerable<Ticket>> GetTicketsByTicketTypeAsync(int ticketTypeId);
		Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId);
	}
}
