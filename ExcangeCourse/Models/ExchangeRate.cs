namespace ExcangeCourse.Models;

public class ExchangeRate
{
    public int Id { get; set; }
    public int BaseCurrencyId { get; set; }
    public int TargetCurrencyId { get; set; }
    public decimal Rate { get; set; }
}