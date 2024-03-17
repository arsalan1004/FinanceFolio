using FinanceFolio.Data;
using FinanceFolio.Models;
using FinanceFolio.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceFolio.Controllers.LoginSignup;

[ApiController]
[Route("/")]
public class LoginSignupController : Controller
{
    private readonly Authentication _firebaseAuth;
    private readonly FinanceFolioContext _financeFolioContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginSignupController(Authentication firebaseAuth, FinanceFolioContext financeFolioContext, 
        IHttpContextAccessor httpContextAccessor)
    {
        this._firebaseAuth = firebaseAuth;
        this._financeFolioContext = financeFolioContext;
        this._httpContextAccessor = httpContextAccessor;    
    }


    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignupUser([FromBody] SignupDto userData)
    {
        try
        {
            var user = new User
            {
                Username = userData.Username,
                Email = userData.Email,
                Password = userData.Password
            };
            
            _financeFolioContext.User.Add(user);
            var token = await _firebaseAuth.Signup(userData.Email, userData.Password);

            if (token is null) return BadRequest();
            _httpContextAccessor.HttpContext?.Session.SetString("token", token);
            await _financeFolioContext.SaveChangesAsync();
            return Ok(userData);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }   
    }
    
    [HttpPost]
    [Route("/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto userData)
    {
        try
        {
            var token = await _firebaseAuth.Login(userData.Email, userData.Password);

            if (token is null) return BadRequest();
            _httpContextAccessor.HttpContext?.Session.SetString("token", token);
            return Ok(userData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }   
    }
    // [HttpGet]
    // [Authorize]
    // [Route("/user/{id:int}")]
    // public async Task<IActionResult> GetUser([FromRoute] int id)
    // {
    //     var user = await _financeFolioContext.FindAsync<User>(id);
    //     return Ok(id);
    // }
}