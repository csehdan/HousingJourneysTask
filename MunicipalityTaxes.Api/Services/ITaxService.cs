using MunicipalityTaxes.Domain.DTOs;
using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Api.Services
{
	public interface ITaxService
	{
		Task<IEnumerable<TaxRecordDto>> GetTaxRecordsAsync(string municipalityName, DateOnly queryDate);
		Task<decimal?> GetEffectiveTaxRateAsync(string municipalityName, DateOnly queryDate);
		Task<bool> AddTaxRecordAsync(MunicipalityTaxRecord record);
	}
}
