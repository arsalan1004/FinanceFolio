using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models;

public class Account
{
    [Required] 
    public int accountId { get; set; }
    public int balance { get; set; }
    [Required]
    public string accountType { get; set; }
    public int userId { get; set; }
    
    //Navigation Property
    public User AccountHolder { get; set; }
}