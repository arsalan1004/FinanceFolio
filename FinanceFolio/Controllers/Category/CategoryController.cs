using FinanceFolio.Data;
using FinanceFolio.Models.DTO.CategoryDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Controllers.Category;

[ApiController]
public class CategoryController : Controller
{
    private readonly FinanceFolioContext _financeFolioContext;

    public CategoryController(FinanceFolioContext financeFolioContext)
    {
        this._financeFolioContext = financeFolioContext;
    }
    
    // GET ALL CATEGORIES
    [HttpGet, Authorize, Route("/category")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var incomeCategories =
                await _financeFolioContext.Category.Where(cat => cat.categoryType == "Income").ToListAsync();
            var expenseCategories =
                await _financeFolioContext.Category.Where(cat => cat.categoryType == "Expenses").ToListAsync();

            if (!incomeCategories.Any() || !expenseCategories.Any())
            {
                return NotFound("Expense or income categories not found");
            }
            
            return Ok(new
            {
               IncomeCategories = incomeCategories,
               ExpenseCategories = expenseCategories
            });

        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest(ex.Message);
        }
    }
    
    // ADD A NEW CATEGORY
    [HttpPost, Authorize, Route("/newCategory")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
    {
        try
        {
            var category = new Models.Category
            {
                categoryName = categoryDto.categoryName,
                categoryType = categoryDto.categoryType
            };

            _financeFolioContext.Category.Add(category);
            await _financeFolioContext.SaveChangesAsync();

            return Ok(category);
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest(ex.Message);
        }
    }
    
}