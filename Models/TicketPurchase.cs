using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
	public class TicketPurchase
	{
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public int Quantity { get; set; }

		public TicketWithoutId ToTicketWithoutId()
		{
			return new TicketWithoutId
			{
				TicketTypeId = TicketTypeId,
				CustomerId = CustomerId,
				PurchaseDate = null,
				Quantity = Quantity,
				Status = "Purchased"
			};
		}
	}
}
