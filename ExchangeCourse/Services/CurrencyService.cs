using ExchangeCourse.Abstractions;
using ExchangeCourse.Database;
using ExchangeCourse.Database.Entities;
using ExchangeCourse.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeCourse.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ApplicationDbContext _context;

    public CurrencyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Currency>> GetCurrencies()
    {
        var currencyEntities = await _context.Currencies.ToListAsync();
        var currencies = currencyEntities
            .Select(c => new Currency { Id = c.Id, Code = c.Code, FullName = c.FullName, Sign = c.Sign }).ToList();
        return currencies;
    }

    public async Task<Currency> GetCurrency(string currencyCode)
    {
        var currencyEntity = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
        if (currencyEntity == null)
        {
            throw new KeyNotFoundException($"Currency code {currencyCode} not found");
        }
        var currency = new Currency {Id = currencyEntity.Id, Code = currencyCode, FullName = currencyEntity.FullName, Sign = currencyEntity.Sign};
        return currency;
    }

    public async Task<Currency> CreateCurrency(Currency currency)
    {
        var currencyEntity = new CurrencyEntity
        {
            Code = currency.Code,
            FullName = currency.FullName,
            Sign = currency.Sign
        };
        try
        {
            _context.Currencies.Add(currencyEntity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        await _context.SaveChangesAsync();
        return currency;
    }
}