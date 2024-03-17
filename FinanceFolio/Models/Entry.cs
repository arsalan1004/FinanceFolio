using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models;

public class Entry
{
    [Required]
    public int entryId { get; set; }
    
    [Required, MinLength(1, ErrorMessage = "Enter a valid entry type")]
    public string entryType { get; set; }
    
    [Required]
    public double amount { get; set; }
    public string description { get; set; }
    public int categoryId { get; set; }
    
    // NAVIGATION PROPERTY
    public Category category { get; set; }
}