namespace ConcertTicketApp.Models
{
	public class TicketType
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ConcertId { get; set; }
		public decimal Price { get; set; }
		public int Capacity { get; set; }
	}
}
