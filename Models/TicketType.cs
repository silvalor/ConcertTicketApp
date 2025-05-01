using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConcertTicketApp.Models
{
	public class TicketType
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int ConcertId { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public int Capacity { get; set; }
	}
}
