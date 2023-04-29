using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
	public class OnlineShopDbContext : DbContext
	{
		public OnlineShopDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Admin> Admins { get; set; }

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Seller> Sellers { get; set; }

		public DbSet<Article> Articles { get; set; }

		public DbSet<Order> Orders { get; set; }

		public DbSet<Item> Items { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineShopDbContext).Assembly);
		}
	}
}
