using FinanceFolio.Data;
using FinanceFolio.Models;
using FinanceFolio.Models.DTO;
using FinanceFolio.Models.DTO.AccountDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceFolio.Controllers.Accounts;

[ApiController, Route("/accounts")]
public class AccountsController: Controller
{
    private readonly FinanceFolioContext _financeFolioContext;
    
    public AccountsController(FinanceFolioContext financeFolioContext)
    {
        this._financeFolioContext = financeFolioContext;
    }
    
    // GET ALL THE ACCOUNTS OF A USER
    [HttpGet, Authorize, Route("/{userId:int}")]
    public async Task<IActionResult> AccountDetails([FromRoute] int userId)
    {
        try
        {
            var accountsQuery = await (from acc in _financeFolioContext.Accounts
                where acc.userId == userId
                select acc).ToListAsync();

            if (accountsQuery.Count() == 0) return NotFound();

            IEnumerable<GetAccountsDto> accountDto =
                from acc in accountsQuery
                select new GetAccountsDto
                {
                    accountType = acc.accountType,
                    balance = acc.balance,
                    userId = acc.userId
                };
            
            return Ok(accountDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // CREATE A NEW ACCOUNT
    [HttpPost, Authorize, Route("/new")]
    public async Task<IActionResult> AddAccount([FromBody] PostAccountDto postAccount)
    {
        try
        {
            var userAccount = new Account
            {
                balance = 0,
                accountType = postAccount.accountType,
                userId = postAccount.userId
            };

            _financeFolioContext.Accounts.Add(userAccount);
            await _financeFolioContext.SaveChangesAsync();

            return Ok(userAccount);
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