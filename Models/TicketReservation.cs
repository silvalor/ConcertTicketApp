using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
	public class TicketReservation
	{
		public int CustomerId { get; set; }
		public DateTime? ReserveDate { get; set; }
		public TimeSpan? ReserveDuration { get; set; }
		public int Quantity { get; set; }

		public Ticket ToTicket(int id, int ticketTypeId)
		{
			return new Ticket
			{
				Id = id,
				TicketTypeId = ticketTypeId,
				CustomerId = CustomerId,
				PurchaseDate = null,
				Quantity = Quantity,
				PurchaseTotal = 0,
				Status = "Reserved"
			};
		}
	}
}
