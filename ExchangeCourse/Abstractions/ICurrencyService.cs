using ExchangeCourse.Models;

namespace ExchangeCourse.Abstractions;

public interface ICurrencyService
{
    public Task<ICollection<Currency>> GetCurrencies();
    public Task<Currency> GetCurrency(string currencyCode);
    public Task<Currency> CreateCurrency(Currency currency);
}