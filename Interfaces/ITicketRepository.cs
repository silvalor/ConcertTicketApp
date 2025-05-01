namespace ConcertTicketApp.Interfaces
{
	public interface ITicketRepository
	{
		Task<AddTicketResult> AddTicketAsync(TicketWithoutId ticket);
		Task DeleteTicketAsync(int id);
		Task<IEnumerable<Ticket>> GetAllTicketsAsync();
		Task<Ticket> GetTicketByIdAsync(int id);
		Task UpdateTicketAsync(Ticket ticket);
		Task<IEnumerable<Ticket>> GetTicketsByTicketTypeAsync(int ticketTypeId);
		Task<IEnumerable<Ticket>> GetTicketsByCustomerIdAsync(int customerId);
	}

	public class AddTicketResult
	{
		public enum AddTicketStatus
		{
			Success,
			InsufficientTickets,
			EventNotFound
		}

		public AddTicketStatus Status { get; set; }
		public Ticket NewTicket { get; set; }
	}
}
