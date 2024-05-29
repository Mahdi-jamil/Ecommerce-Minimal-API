using Ecommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Data
{

	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }
		public DbSet<Product> products { get; set; }
		public DbSet<Category> categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Category)
				.WithMany()
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);
			// this is so that when we delete a category, we delete all products of that category
		}

	}

}