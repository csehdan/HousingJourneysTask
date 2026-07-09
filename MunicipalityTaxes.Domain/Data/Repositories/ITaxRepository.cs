using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Domain.Data.Repositories
{
	public interface ITaxRepository
	{
		Task<IEnumerable<MunicipalityTaxRecord>> GetByMunicipalityAndDateAsync(
			string municipalityName, DateOnly queryDate);

		Task<bool> AddAsync(MunicipalityTaxRecord record);
	}
}
