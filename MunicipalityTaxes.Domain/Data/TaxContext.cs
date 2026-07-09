using Microsoft.EntityFrameworkCore;
using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Domain.Data
{
	public class TaxContext : DbContext
	{
		public DbSet<MunicipalityTaxRecord> MunicipalityTaxRecords => Set<MunicipalityTaxRecord>();

		public TaxContext(DbContextOptions<TaxContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<MunicipalityTaxRecord>(entity =>
			{
				entity.HasKey(e => e.Id);

				// Index for efficient date range queries
				entity.HasIndex(e => new { e.MunicipalityName, e.PeriodStart, e.PeriodEnd });

				entity.Property(e => e.MunicipalityName)
					  .IsRequired()
					  .HasMaxLength(100);
			});

			// Seed demo data (Copenhagen example from requirements)
			modelBuilder.Entity<MunicipalityTaxRecord>().HasData(
				new MunicipalityTaxRecord
				{
					Id = 1,
					MunicipalityName = "Copenhagen",
					TaxRate = 0.2m,
					PeriodStart = new DateOnly(2024, 1, 1),
					PeriodEnd = new DateOnly(2024, 12, 31)
				},
				new MunicipalityTaxRecord
				{
					Id = 2,
					MunicipalityName = "Copenhagen",
					TaxRate = 0.4m,
					PeriodStart = new DateOnly(2024, 5, 1),
					PeriodEnd = new DateOnly(2024, 5, 31)
				},
				new MunicipalityTaxRecord
				{
					Id = 3,
					MunicipalityName = "Copenhagen",
					TaxRate = 0.1m,
					PeriodStart = new DateOnly(2024, 1, 1),
					PeriodEnd = new DateOnly(2024, 1, 1)
				},
				new MunicipalityTaxRecord
				{
					Id = 4,
					MunicipalityName = "Copenhagen",
					TaxRate = 0.1m,
					PeriodStart = new DateOnly(2024, 12, 25),
					PeriodEnd = new DateOnly(2024, 12, 25)
				}
			);
		}
	}
}
