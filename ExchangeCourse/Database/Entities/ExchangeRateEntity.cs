using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExchangeCourse.Database.Entities;

[Table("ExchangeRates")]
public class ExchangeRateEntity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [ForeignKey("Currencies")]
    [Required]
    [Column("BaseCurrency")]
    public CurrencyEntity BaseCurrency { get; set; }
    
    [ForeignKey("Currencies")]
    [Required]
    [Column("TargetCurrency")]
    public CurrencyEntity TargetCurrency { get; set; }
    
    [Required]
    [Column("Rate")]
    public decimal Rate { get; set; }
}