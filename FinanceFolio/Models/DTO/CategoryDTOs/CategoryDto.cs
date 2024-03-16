using System.ComponentModel.DataAnnotations;

namespace FinanceFolio.Models.DTO.CategoryDTOs;

public class CategoryDto
{
    [Required, MinLength(1, ErrorMessage = "Category name cannot be an empty string")]
    public string categoryName { get; set; }
    [Required, MinLength(1, ErrorMessage = "Category type cannot be an empty string")]
    public string categoryType { get; set; }
}