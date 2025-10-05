namespace ExchangeCourse.Contracts;

public record ExchangeRateResponse(int id, CurrencyResponse baseCurrency, CurrencyResponse targetCurrency, decimal rate);