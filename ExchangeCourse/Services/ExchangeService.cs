using ExchangeCourse.Abstractions;
using ExchangeCourse.Models;

namespace ExchangeCourse.Services;

public class ExchangeService : IExchangeService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeService> _logger;

    public ExchangeService(IExchangeRateService exchangeRateService, ILogger<ExchangeService> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }
    
    public async Task<Exchange> GetExchange(string baseCode, string targetCode, decimal amount)
    {
        try
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRateById(baseCode, targetCode);
            var convertedAmount = exchangeRate.Rate * amount;
            var exchange = new Exchange
            {
                BaseCurrency = exchangeRate.BaseCurrency,
                TargetCurrency = exchangeRate.TargetCurrency,
                Amount = amount,
                Rate = exchangeRate.Rate,
                ConvertedAmount = convertedAmount
            };
            return exchange;
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation("Make another way to exchange");
        }

        try
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRateById(targetCode, baseCode);
            var convertedAmount = (1 /exchangeRate.Rate) * amount;
            var exchange = new Exchange
            {
                BaseCurrency = exchangeRate.TargetCurrency,
                TargetCurrency = exchangeRate.BaseCurrency,
                Amount = amount,
                Rate = 1 / exchangeRate.Rate,
                ConvertedAmount = convertedAmount
            };
            return exchange;
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation("Make another way to exchange");
        }

        try
        {
            var exchangeRateFirst = await _exchangeRateService.GetExchangeRateById("USD", baseCode);
            var exchangeRateSecond = await _exchangeRateService.GetExchangeRateById("USD", targetCode);
            var rate = exchangeRateSecond.Rate / exchangeRateFirst.Rate;
            var convertedAmount = rate * amount;
            var exchange = new Exchange
            {
                BaseCurrency = exchangeRateFirst.TargetCurrency,
                TargetCurrency = exchangeRateSecond.TargetCurrency,
                Rate = rate,
                ConvertedAmount = convertedAmount,
                Amount = amount
            };
            return exchange;
        }
        catch (ArgumentException e)
        {
            _logger.LogError("Can't get exchange rate");
            throw;
        }
    }
}