using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeCourse.Database.Entities;

[Table("ExchangeRates")]
public class ExchangeRateEntity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    

    [Required]
    [Column("BaseCurrencyId")]
    public int BaseCurrencyId { get; set; }
    public CurrencyEntity BaseCurrency { get; set; }
    
    [Required]
    [Column("TargetCurrencyId")]
    public int TargetCurrencyId { get; set; }
    public CurrencyEntity TargetCurrency { get; set; }
    
    [Required]
    [Column("Rate")]
    public decimal Rate { get; set; }
}