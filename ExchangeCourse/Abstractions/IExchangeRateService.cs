using ExchangeCourse.Models;

namespace ExchangeCourse.Abstractions;

public interface IExchangeRateService
{
    public Task<ICollection<ExchangeRate>> GetExchangeRates();
    public Task<ExchangeRate> GetExchangeRateById(string baseCurrency, string targetCurrency);
    public Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate);
    public Task<ExchangeRate> UpdateExchangeRate(string baseCurrency, string targetCurrency, decimal rate);
}