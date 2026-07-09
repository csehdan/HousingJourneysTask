namespace MunicipalityTaxes.Domain.DTOs
{
	public record TaxRecordDto(
		int Id,
		string MunicipalityName,
		decimal TaxRate,
		DateOnly PeriodStart,
		DateOnly PeriodEnd
	);
}
