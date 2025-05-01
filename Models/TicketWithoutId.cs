using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
    public class TicketWithoutId
    {
		public int ConcertId { get; set; }
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public string Status { get; set; } // "Reserved", "Purchased", "Cancelled"
		public DateTime? PurchaseDate { get; set; }
		public DateTime? ReserveExpiration { get; set; }
		public int Quantity { get; set; }

		public Ticket ToTicket(int id)
		{
			return new Ticket
			{
				Id = id,
				ConcertId = ConcertId,
				TicketTypeId = TicketTypeId,
				CustomerId = CustomerId,
				PurchaseDate = PurchaseDate,
				ReserveExpiration = ReserveExpiration,
				Quantity = Quantity,
				Status = Status
			};
		}

	}
}
