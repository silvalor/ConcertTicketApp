using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
    public class Ticket
    {
		public int Id { get; set; }
		public int ConcertId { get; set; }
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public string Status { get; set; } // "Reserved", "Purchased", "Cancelled"
		public DateTime? PurchaseDate { get; set; }
		public DateTime? ReserveExpiration { get; set; }
		public int Quantity { get; set; }

		public Ticket CopyWith(
			int? id = null,
			int? ticketTypeId = null, 
			int? customerId = null,
			string status = null,
			DateTime? purchaseDate = null, 
			DateTime? reserveExpiration = null, 
			int? quantity = null,
			decimal? purchaseTotal = null)
		{
			return new Ticket
			{
				Id = id ?? Id,
				TicketTypeId = ticketTypeId ?? TicketTypeId,
				CustomerId = customerId ?? CustomerId,
				Status = status ?? Status,
				PurchaseDate = purchaseDate ?? PurchaseDate,
				ReserveExpiration = reserveExpiration ?? ReserveExpiration,
				Quantity = quantity ?? Quantity,
			};
		}

	}
}
