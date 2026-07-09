using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxes.Api.Services;
using MunicipalityTaxes.Domain.DTOs;
using MunicipalityTaxes.Domain.Entities;

namespace MunicipalityTaxes.Api.Controllers
{
	[ApiController]
	[Route("api/taxes")]

	public class TaxesController(ITaxService taxService) : ControllerBase
	{
		[HttpGet("{municipalityName}/date/{queryDate:datetime}")]
		[ProducesResponseType<IEnumerable<TaxRecordDto>>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTaxRecords(string municipalityName, DateOnly queryDate)
		{
			var records = await taxService.GetTaxRecordsAsync(municipalityName, queryDate);

			if (!records.Any())
			{
				return NotFound($"No tax records found for {municipalityName} on {queryDate}");
			}

			return Ok(records);
		}

		[HttpGet("{municipalityName}/rate/{queryDate:datetime}")]
		[ProducesResponseType<decimal?>(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetEffectiveTaxRate(string municipalityName, DateOnly queryDate)
		{
			var rate = await taxService.GetEffectiveTaxRateAsync(municipalityName, queryDate);

			if (rate == null)
			{
				return NotFound($"No effective tax rate found for {municipalityName} on {queryDate}");
			}

			return Ok(new { municipality = municipalityName, date = queryDate, rate });
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddTaxRecord([FromBody] MunicipalityTaxRecord record)
		{
			try
			{
				var success = await taxService.AddTaxRecordAsync(record);
				return success ? CreatedAtAction(nameof(GetTaxRecords),
					new { municipalityName = record.MunicipalityName, queryDate = record.PeriodStart },
					record)
					: BadRequest("Failed to add tax record");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
