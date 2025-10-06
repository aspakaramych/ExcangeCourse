using ExchangeCourse.Abstractions;
using ExchangeCourse.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCourse.Controllers;

[ApiController]
[Route("api")]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeService;
    private readonly ILogger<ExchangeController> _logger;

    public ExchangeController(IExchangeService exchangeService, ILogger<ExchangeController> logger)
    {
        _exchangeService = exchangeService;
        _logger = logger;
    }
    
    [HttpGet("/exchange+from={from}&to={to}&amount={amount}")]
    public async Task<ActionResult<ExchangeResponse>> GetExchange(string from, string to, decimal amount)
    {
        try
        {
            var exchange = await _exchangeService.GetExchange(from, to, amount);
            return Ok(exchange.ToContract());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }
}