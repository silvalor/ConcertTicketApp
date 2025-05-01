using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
    public class TicketWithoutId
    {
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public string Status { get; set; } // "Reserved", "Purchased", "Cancelled"
		public DateTime? PurchaseDate { get; set; }
		public DateTime? ReserveDate { get; set; }
		public TimeSpan? ReserveDuration { get; set; }
		public int Quantity { get; set; }
		public decimal PurchaseTotal { get; set; }

		public Ticket ToTicket(int id)
		{
			return new Ticket
			{
				Id = id,
				TicketTypeId = TicketTypeId,
				CustomerId = CustomerId,
				PurchaseDate = PurchaseDate,
				ReserveDate = ReserveDate,
				ReserveDuration = ReserveDuration,
				Quantity = Quantity,
				PurchaseTotal = PurchaseTotal,
				Status = Status
			};
		}

	}
}
