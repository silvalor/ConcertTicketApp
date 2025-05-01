using ConcertTicketApp.Models;

namespace ConcertTicketApp
{
	public class TicketReservation
	{
		public int ConcertId { get; set; }
		public int TicketTypeId { get; set; }
		public int CustomerId { get; set; }
		public int? MinutesReserved { get; set; }
		public int Quantity { get; set; }

		public TicketWithoutId ToTicketWithoutId(DateTime startingTime)
		{
			return new TicketWithoutId
			{
				ConcertId = ConcertId,
				TicketTypeId = TicketTypeId,
				CustomerId = CustomerId,
				ReserveExpiration = startingTime + TimeSpan.FromMinutes(MinutesReserved ?? 0),
				PurchaseDate = null,
				Quantity = Quantity,
				Status = "Reserved"
			};
		}
	}
}
