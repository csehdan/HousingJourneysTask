using Microsoft.EntityFrameworkCore;
using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Domain.Data.Repositories
{
	public class TaxRepository : ITaxRepository
	{
		private readonly TaxContext _context;

		public TaxRepository(TaxContext context) => _context = context;

		public async Task<IEnumerable<MunicipalityTaxRecord>> GetByMunicipalityAndDateAsync(
			string municipalityName, DateOnly queryDate)
		{
			return await _context.MunicipalityTaxRecords
				.Where(r => EF.Functions.Like(r.MunicipalityName, municipalityName)
						 && r.PeriodStart <= queryDate
						 && r.PeriodEnd >= queryDate)
				.ToListAsync();
		}

		public async Task<bool> AddAsync(MunicipalityTaxRecord record)
		{
			_context.MunicipalityTaxRecords.Add(record);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
