using ExchangeCourse.Contracts;

namespace ExchangeCourse.Models;

public class Currency
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public string Sign { get; set; }

    public CurrencyResponse toContract()
    {
        return new CurrencyResponse(Id, FullName, Code, Sign);
    }

    public override string ToString()
    {
        return $"Id: {Id}, Code: {Code}, FullName: {FullName}, Sign: {Sign}";
    }
}