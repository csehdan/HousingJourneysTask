using MunicipalityTaxes.Domain.Data.Repositories;
using MunicipalityTaxes.Domain.DTOs;
using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Api.Services
{
	public class TaxService : ITaxService
	{
		private readonly ITaxRepository _repository;

		public TaxService(ITaxRepository repository) => _repository = repository;

		public async Task<IEnumerable<TaxRecordDto>> GetTaxRecordsAsync(string municipalityName, DateOnly queryDate)
		{
			var records = await _repository.GetByMunicipalityAndDateAsync(municipalityName, queryDate);

			// todo: maybe valide output data too, if an interval really represents a week/month/etc, if required

			return records.Select(r => new TaxRecordDto(
					r.Id,
					r.MunicipalityName,
					r.TaxRate,
					r.PeriodStart,
					r.PeriodEnd
				));
		}

		public async Task<decimal?> GetEffectiveTaxRateAsync(string municipalityName, DateOnly queryDate)
		{
			var applicableRates = await GetTaxRecordsAsync(municipalityName, queryDate);

			if (!applicableRates.Any())
			{
				return null;
			}

			// todo: get clarity for determining which is the "most effective" rate for a given date

			return applicableRates.OrderBy(r => r.PeriodEnd.DayNumber - r.PeriodStart.DayNumber).FirstOrDefault()?.TaxRate;
		}

		public async Task<bool> AddTaxRecordAsync(MunicipalityTaxRecord record)
		{
			// todo: handle duplicates (existing interval for a municipality), either block them, or find a rule
			// todo: maybe add validation to check if the given interval is really a week, month etc


			if (record.PeriodStart > record.PeriodEnd)
			{
				throw new ArgumentException("PeriodStart must be before or equal to PeriodEnd");
			}

			return await _repository.AddAsync(record);
		}
	}
}
