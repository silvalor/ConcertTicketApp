using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
    public class Ticket
    {
		public int Id { get; set; }
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public DateTime PurchaseDate { get; set; }
		public int Quantity { get; set; }
		public decimal PurchaseTotal { get; set; }
		public string Status { get; set; } // "Reserved", "Purchased", "Cancelled"
	}
}
