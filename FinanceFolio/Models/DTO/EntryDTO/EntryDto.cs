using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models.DTO.EntryDTO;

public class EntryDto
{
    [Required, MinLength(1, ErrorMessage = "Enter a valid entry type")]
    public string entryType { get; set; }
    
    [Required]
    public double amount { get; set; }
    public string description { get; set; }
    public DateOnly dateofEntry { get; set; }
    public int categoryId { get; set; }
    public int accountId { get; set; }
}