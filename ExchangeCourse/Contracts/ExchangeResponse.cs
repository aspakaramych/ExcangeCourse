namespace ExchangeCourse.Contracts;

public record ExchangeResponse(
    CurrencyResponse baseCurrency,
    CurrencyResponse targetCurrency,
    decimal rate,
    decimal amount,
    decimal convertedAmount);