namespace ExchangeCourse.Contracts;

public record CurrencyRequest(
    string name,
    string code,
    string sign);