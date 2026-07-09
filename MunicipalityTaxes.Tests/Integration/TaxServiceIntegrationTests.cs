using Microsoft.EntityFrameworkCore;
using MunicipalityTaxes.Domain.Data;
using MunicipalityTaxes.Domain.Data.Repositories;
using MunicipalityTaxes.Api.Services;

namespace MunicipalityTaxes.Tests.Integration
{
	public class TaxServiceIntegrationTests : IDisposable
	{
		private readonly TaxContext _context;
		private readonly ITaxRepository _repository;
		private readonly ITaxService _service;

		public TaxServiceIntegrationTests()
		{
			var options = new DbContextOptionsBuilder<TaxContext>()
				.UseSqlite("DataSource=:memory:")
				.Options;

			_context = new TaxContext(options);

			_context.Database.OpenConnection();
			_context.Database.EnsureCreated();

			_repository = new TaxRepository(_context);

			_service = new TaxService(_repository);
		}

		[Fact]
		public async Task GetTaxRecords_DailyTax_ReturnsCorrectRecord()
		{
			await _repository.AddAsync(new Domain.Entities.MunicipalityTaxRecord
			{
				MunicipalityName = "Brazeles",
				TaxRate = 0.1m,
				PeriodStart = new DateOnly(2024, 1, 15),
				PeriodEnd = new DateOnly(2024, 1, 15)
			});

			var result = await _service.GetTaxRecordsAsync("Brazeles", new DateOnly(2024, 1, 15));

			Assert.Single(result);
			Assert.Equal(0.1m, result.First().TaxRate);
		}

		[Fact]
		public async Task GetTaxRecords_DateOutsideRange_ReturnsEmpty()
		{
			var result = await _service.GetTaxRecordsAsync("Moscow", new DateOnly(2024, 6, 15));

			Assert.Empty(result);
		}

		[Fact]
		public async Task AddTaxRecord_ValidRecord_ReturnsTrue()
		{
			var validRecord = new Domain.Entities.MunicipalityTaxRecord
			{
				MunicipalityName = "Klaipeda",
				TaxRate = 0.15m,
				PeriodStart = new DateOnly(2024, 1, 1),
				PeriodEnd = new DateOnly(2024, 12, 31)
			};

			var result = await _repository.AddAsync(validRecord);

			Assert.True(result);
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
