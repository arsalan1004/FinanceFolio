using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models;

public class Category
{
    [Required]
    public int categoryId { get; set; }
    [Required]
    public string categoryName { get; set; }
    [Required]
    public string categoryType { get; set; }
}