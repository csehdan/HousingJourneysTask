namespace MunicipalityTaxes.Domain.Entities;

public class MunicipalityTaxRecord
{
	public int Id { get; set; }

	public required string MunicipalityName { get; set; }
	public decimal TaxRate { get; set; }
	public DateOnly PeriodStart { get; set; }
	public DateOnly PeriodEnd { get; set; }
}