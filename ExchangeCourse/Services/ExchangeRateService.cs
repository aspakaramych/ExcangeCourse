using ExchangeCourse.Abstractions;
using ExchangeCourse.Database;
using ExchangeCourse.Database.Entities;
using ExchangeCourse.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeCourse.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(ApplicationDbContext context, ILogger<ExchangeRateService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<ExchangeRate>> GetExchangeRates()
    {
        try
        {
            var exchangeRatesEntities =
                await _context.ExchangeRates
                    .Include(e => e.BaseCurrency)
                    .Include(e => e.TargetCurrency).ToListAsync();
            var echangeRates = exchangeRatesEntities.Select(e => new ExchangeRate
            {
                Id = e.Id,
                BaseCurrency = new Currency
                {
                    Id = e.BaseCurrency.Id,
                    Code = e.BaseCurrency.Code,
                    FullName = e.BaseCurrency.FullName,
                    Sign = e.BaseCurrency.Sign,
                },
                TargetCurrency = new Currency
                {
                    Id = e.TargetCurrency.Id,
                    Code = e.TargetCurrency.Code,
                    FullName = e.TargetCurrency.FullName,
                    Sign = e.TargetCurrency.Sign,
                },
                Rate = e.Rate,
            }).ToList();
            return echangeRates;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        
    }

    public async Task<ExchangeRate> GetExchangeRateById(string baseCurrency, string targetCurrency)
    {
        var exchangeRateEntity = await _context.ExchangeRates
            .Include(e => e.BaseCurrency)
            .Include(e => e.TargetCurrency)
            .FirstOrDefaultAsync(e =>
                e.BaseCurrency.Code == baseCurrency && e.TargetCurrency.Code == targetCurrency);
        if (exchangeRateEntity == null)
        {
            _logger.LogError($"No exchange rate found for {baseCurrency}/{targetCurrency}");
            throw new ArgumentException($"No exchange rate exists with id: {baseCurrency}/{targetCurrency}");
        }

        var exchangeRate = new ExchangeRate
        {
            Id = exchangeRateEntity.Id,
            BaseCurrency = new Currency
            {
                Id = exchangeRateEntity.BaseCurrency.Id,
                Code = exchangeRateEntity.BaseCurrency.Code,
                FullName = exchangeRateEntity.BaseCurrency.FullName,
                Sign = exchangeRateEntity.BaseCurrency.Sign,
            },
            TargetCurrency = new Currency
            {
                Id = exchangeRateEntity.TargetCurrency.Id,
                Code = exchangeRateEntity.TargetCurrency.Code,
                FullName = exchangeRateEntity.TargetCurrency.FullName,
                Sign = exchangeRateEntity.TargetCurrency.Sign,
            },
            Rate = exchangeRateEntity.Rate,
        };
        return exchangeRate;
    }

    public async Task<ExchangeRate> AddExchangeRate(ExchangeRate exchangeRate)
    {
        var exchangeRateEntity = new ExchangeRateEntity
        {
            Id = exchangeRate.Id,
            BaseCurrencyId = exchangeRate.BaseCurrency.Id,
            TargetCurrencyId = exchangeRate.TargetCurrency.Id,
            Rate = exchangeRate.Rate,
        };
        try
        {
            _context.ExchangeRates.Add(exchangeRateEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }

        return exchangeRate;
    }

    public async Task<ExchangeRate> UpdateExchangeRate(string baseCurrency, string targetCurrency, decimal rate)
    {
        try
        {
            await _context.ExchangeRates
                .Include(e => e.BaseCurrency)
                .Include(e => e.TargetCurrency)
                .Where(e => e.BaseCurrency.Code == baseCurrency && e.TargetCurrency.Code == targetCurrency)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Rate, rate));
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
        var exchangeRate = await GetExchangeRateById(baseCurrency, targetCurrency);
        return exchangeRate;
    }
}