namespace ConcertTicketApp.Models
{
    public class ConcertWithoutId
    {
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public string Venue { get; set; }
		public string Description { get; set; }

		public Concert AddId(int id)
		{
			return new Concert
			{
				Id = id,
				Name = Name,
				Date = Date,
				Venue = Venue,
				Description = Description
			};
		}
	}
}
