using ExchangeCourse.Contracts;

namespace ExchangeCourse.Models;

public class Exchange
{
    public Currency BaseCurrency { get; set; }
    public Currency TargetCurrency { get; set; }
    public decimal Rate { get; set; }
    public decimal Amount { get; set; }
    public decimal ConvertedAmount { get; set; }

    public ExchangeResponse ToContract()
    {
        return new ExchangeResponse(BaseCurrency.ToContract(), TargetCurrency.ToContract(), Rate, Amount, ConvertedAmount);
    }
}