namespace ExchangeCourse.Contracts;

public record CurrencyResponse(
    int id,
    string name,
    string code,
    string sign
);