using ExchangeCourse.Abstractions;
using ExchangeCourse.Contracts;
using ExchangeCourse.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCourse.Controllers;

[ApiController]
[Route("api")]
public class ExchangeRatesController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(IExchangeRateService exchangeRateService, ICurrencyService currencyService, ILogger<ExchangeRatesController> logger)
    {
        _exchangeRateService = exchangeRateService;
        _currencyService = currencyService;
        _logger = logger;
    }

    [HttpGet("/exchangeRates")]
    public async Task<ActionResult<IEnumerable<ExchangeRateResponse>>> Get()
    {
        try
        {
            var exchangeRates = await _exchangeRateService.GetExchangeRates();
            var response = exchangeRates.Select(e => e.ToContract());
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("/exchangeRates/{baseCode}+{targetCode}")]
    public async Task<ActionResult<ExchangeRateResponse>> Get(string baseCode, string targetCode)
    {
        if (string.IsNullOrEmpty(baseCode) || string.IsNullOrEmpty(targetCode))
        {
            return BadRequest("one of code is empty");
        }

        try
        {
            var baseCurrency = await _currencyService.GetCurrency(baseCode);
            var targetCurrency = await _currencyService.GetCurrency(targetCode);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound("one or more currencies are not found");
        }
        
        try
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRateById(baseCode, targetCode);
            var response = exchangeRate.ToContract();
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
    
    [HttpPost("/exchangeRates")]
    public async Task<ActionResult<ExchangeRateResponse>> Create(string baseCode, string targetCode, decimal rate)
    {
        if (string.IsNullOrEmpty(baseCode) || string.IsNullOrEmpty(targetCode))
        {
            return BadRequest("one of code is empty");
        }

        try
        {
            var baseCurrency = await _currencyService.GetCurrency(baseCode);
            var targetCurrency = await _currencyService.GetCurrency(targetCode);
            var newExchangeRate = new ExchangeRate
            {
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                Rate = rate
            };

            var exchangeRate = await _exchangeRateService.AddExchangeRate(newExchangeRate);
            var response = exchangeRate.ToContract();
            return Ok(response);
        }
        catch (KeyNotFoundException keyNotFoundException)
        {
            return NotFound("one of currency not found");
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(409);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(409, "exchange rate already exists");
        }
    }
    
    [HttpPatch("/exchangeRates/{baseCode}+{targetCode}")]
    public async Task<ActionResult<ExchangeRateResponse>> Update(string baseCode, string targetCode, decimal rate)
    {
        if (string.IsNullOrEmpty(baseCode) || string.IsNullOrEmpty(targetCode) || rate == 0)
        {
            return BadRequest("one of code is empty");
        }
        try
        {
            
            var exchangeRate = await _exchangeRateService.UpdateExchangeRate(baseCode, targetCode, rate);
            var response = exchangeRate.ToContract();
            return Ok(response);
        }
        catch (KeyNotFoundException keyNotFoundException)
        {
            _logger.LogError(keyNotFoundException, keyNotFoundException.Message);
            return NotFound("one of currency not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
}