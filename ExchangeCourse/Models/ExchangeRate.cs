namespace ExchangeCourse.Models;

public class ExchangeRate
{
    public int Id { get; set; }
    public Currency BaseCurrency { get; set; }
    public Currency TargetCurrency { get; set; }
    public decimal Rate { get; set; }
}