using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExchangeCourse.Database.Entities;

[Table("Currencies")]
public class CurrencyEntity
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }
    
    [Column("Code")]
    [Required]
    public string Code { get; set; }
    
    [Column("FullName")]
    [Required]
    public string FullName { get; set; }
    
    [Column("Sign")]
    [Required]
    public string Sign { get; set; }
}