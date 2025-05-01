namespace ConcertTicketApp.Models
{
	public class TicketType
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int ConcertId { get; set; }
		public Concert Concert { get; set; }
		public decimal Price { get; set; }
		public int Available { get; set; }
	}
}
