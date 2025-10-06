using ExchangeCourse.Models;

namespace ExchangeCourse.Abstractions;

public interface IExchangeService
{
    public Task<Exchange> GetExchange(string baseCode, string targetCode, decimal amount);
}