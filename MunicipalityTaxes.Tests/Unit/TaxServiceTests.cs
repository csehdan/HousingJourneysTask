using Moq;
using MunicipalityTaxes.Domain.Data.Repositories;
using MunicipalityTaxes.Api.Services;
using MunicipalityTaxes.Domain.Entities;


namespace MunicipalityTaxes.Tests.Unit
{
	public class TaxServiceTests
	{
		[Fact]
		public async Task AddTaxRecord_InvalidPeriod_ThrowsArgumentException()
		{
			var mockRepository = new Mock<ITaxRepository>();
			var service = new TaxService(mockRepository.Object);

			var invalidRecord = new MunicipalityTaxRecord
			{
				MunicipalityName = "Vilnius",
				TaxRate = 0.1m,
				PeriodStart = new DateOnly(2026, 7, 6),
				PeriodEnd = new DateOnly(2026, 6, 7)
			};

			await Assert.ThrowsAsync<ArgumentException>(() => service.AddTaxRecordAsync(invalidRecord));

			mockRepository.Verify(r => r.AddAsync(It.IsAny<MunicipalityTaxRecord>()), Times.Never);
		}
	}
}
