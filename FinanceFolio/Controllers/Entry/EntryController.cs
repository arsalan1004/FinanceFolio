using FinanceFolio.Data;
using FinanceFolio.Models.DTO.EntryDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Controllers.Entry;

[ApiController]
public class EntryController : Controller
{
    private readonly FinanceFolioContext _financeFolioContext;

    public EntryController(FinanceFolioContext financeFolioContext)
    {
        this._financeFolioContext = financeFolioContext;
    }

    // ADDING AN ENTRY IN AN ACCOUNT
    [HttpPost, Authorize, Route("/entry/new")]
    public async Task<IActionResult> AddEntry([FromBody] EntryDto? entryDto)
    {
        if (entryDto == null) return BadRequest("No entryDto was provided");
        var entry = new Models.Entry
        {
            entryType = entryDto.entryType,
            amount = entryDto.amount,
            description = entryDto.description,
            dateofEntry = DateOnly.FromDateTime(DateTime.UtcNow),
            categoryId = entryDto.categoryId,
            accountId = entryDto.accountId
        };
        _financeFolioContext.Entry.Add(entry);
        await _financeFolioContext.SaveChangesAsync();
        return Ok(entryDto);
    }

    // GET ALL THE ENTRIES OF AN ACCOUNT
    [HttpGet, Authorize, Route("/entry/all/{id:int}")]
    public async Task<IActionResult> GetAllEntries([FromRoute] int? id)
    {
        if (id == null) return BadRequest("No account Id provided");
        var entries = await _financeFolioContext.Entry.Where(entry => entry.accountId == id).ToListAsync();

        if (!entries.Any()) return NotFound("No entries found, Maybe because of invalid account id");

        return Ok(entries);
    }

    // DELETE A TRANSACTION
    [HttpDelete, Authorize, Route("/entry/delete")]
    public async Task<IActionResult> DeleteEntry([FromBody] Models.Entry? entry)
    {
        if (entry == null) return BadRequest("No entry was given");

        _financeFolioContext.Entry.Remove(entry);
        await _financeFolioContext.SaveChangesAsync();
        return Ok(new EntryDto
        {
            entryType = entry.entryType,
            amount = entry.amount,
            description = entry.description,
            dateofEntry = entry.dateofEntry,
            categoryId = entry.categoryId,
            accountId = entry.accountId
        });
    }

    // EDIT A TRANSACTION
    [HttpPut, Authorize, Route("/entry/update")]
    public async Task<IActionResult> UpdateEntry([FromBody] Models.Entry? entry)
    {
        if (entry == null) return BadRequest("No entry was given");

        _financeFolioContext.Entry(entry).State = EntityState.Modified;
        await _financeFolioContext.SaveChangesAsync();
        return Ok(new EntryDto
        {
            entryType = entry.entryType,
            amount = entry.amount,
            description = entry.description,
            dateofEntry = entry.dateofEntry,
            categoryId = entry.categoryId,
            accountId = entry.accountId
        });
    }
}