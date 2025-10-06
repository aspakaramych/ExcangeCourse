using ExchangeCourse.Abstractions;
using ExchangeCourse.Contracts;
using ExchangeCourse.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCourse.Controllers;

[ApiController]
[Route("api")]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;
    private readonly ILogger<CurrencyController> _logger;

    public CurrencyController(ICurrencyService currencyService, ILogger<CurrencyController> logger)
    {
        _currencyService = currencyService;
        _logger = logger;
    }
    
    [HttpGet("/currencies")]
    public async Task<ActionResult<ICollection<CurrencyResponse>>> Get()
    {
        try
        {
            var currencies = await _currencyService.GetCurrencies();
            var currenciesResponse = currencies.Select(c => c.ToContract());
            return Ok(currenciesResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
    [HttpGet("/currency/{code}")]
    public async Task<ActionResult<CurrencyResponse>> GetCurrency(string? code)
    {
        if (code == null)
        {
            return BadRequest("code is empty");
        }

        try
        {
            var currency = await _currencyService.GetCurrency(code);
            var currencyResponse = currency.ToContract();
            return Ok(currencyResponse);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError(e, e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
    
    [HttpPost("/currencies")]
    public async Task<ActionResult<CurrencyResponse>> CreateCurrency([FromBody] CurrencyRequest currencyRequest)
    {
        if (string.IsNullOrEmpty(currencyRequest.name) || string.IsNullOrEmpty(currencyRequest.code) ||
            string.IsNullOrEmpty(currencyRequest.sign))
        {
            return BadRequest("one or more fields are required");
        }
        
        var currencyModel = new Currency
        {
            FullName = currencyRequest.name,
            Code = currencyRequest.code,
            Sign = currencyRequest.code
        };
        try
        {
            var currency = await _currencyService.CreateCurrency(currencyModel);
            var currencyResponse = currency.ToContract();
            return StatusCode(201, currencyResponse);
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(409, "this currency already exists");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
}