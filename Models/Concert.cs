namespace ConcertTicketApp.Models
{
    public class Concert
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Venue { get; set; }
		public string Description { get; set; }

    }
}
