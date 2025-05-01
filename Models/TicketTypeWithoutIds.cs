namespace ConcertTicketApp.Models
{
	public class TicketTypeWithoutIds
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Capacity { get; set; }

		public TicketType AddIds(int concertId, int ticketTypeId)
		{
			return new TicketType
			{
				Id = ticketTypeId,
				ConcertId = concertId,
				Name = Name,
				Price = Price,
				Capacity = Capacity
			};
		}
	}
}
