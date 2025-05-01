using ConcertTicketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicketApp.Db
{
	public class ConcertDbContext : DbContext
	{
		public ConcertDbContext(DbContextOptions<ConcertDbContext> options)
			: base(options) { }

		public DbSet<Concert> Concerts { get; set; }
		public DbSet<TicketType> TicketTypes { get; set; }
		public DbSet<Ticket> Tickets { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}


	}
}
