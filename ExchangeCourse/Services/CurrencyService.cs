using ExchangeCourse.Abstractions;
using ExchangeCourse.Database;
using ExchangeCourse.Database.Entities;
using ExchangeCourse.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeCourse.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CurrencyService> _logger;

    public CurrencyService(ApplicationDbContext context, ILogger<CurrencyService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<Currency>> GetCurrencies()
    {
        try
        {
            var currencyEntities = await _context.Currencies.ToListAsync();
            var currencies = currencyEntities
                .Select(c => new Currency { Id = c.Id, Code = c.Code, FullName = c.FullName, Sign = c.Sign }).ToList();
            return currencies;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        
    }

    public async Task<Currency> GetCurrency(string currencyCode)
    {
        var currencyEntity = await _context.Currencies.AsNoTracking().FirstOrDefaultAsync(c => c.Code == currencyCode);
        if (currencyEntity == null)
        {
            _logger.LogError($"Currency {currencyCode} not found");
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
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        var currencyNewEntity = await _context.Currencies.FirstAsync(c => c.Code == currency.Code);
        var currencyNew = new Currency
        {
            Id = currencyNewEntity.Id,
            Code = currencyNewEntity.Code,
            FullName = currencyNewEntity.FullName,
            Sign = currencyNewEntity.Sign
        };
        return currencyNew;
    }
}