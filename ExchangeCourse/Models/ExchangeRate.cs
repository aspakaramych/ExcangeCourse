using ExchangeCourse.Contracts;

namespace ExchangeCourse.Models;

public class ExchangeRate
{
    public int Id { get; set; }
    public Currency BaseCurrency { get; set; }
    public Currency TargetCurrency { get; set; }
    public decimal Rate { get; set; }

    public ExchangeRateResponse toContract()
    {
        return new ExchangeRateResponse(Id, BaseCurrency.toContract(), TargetCurrency.toContract(), Rate);
    }

    public override string ToString()
    {
        return $"Id: {Id}, BaseCurrency: {BaseCurrency.ToString()}, TargetCurrency: {TargetCurrency.ToString()}, Rate: {Rate}";
    }
}