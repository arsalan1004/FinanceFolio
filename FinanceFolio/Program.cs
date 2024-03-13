using FinanceFolio;
using FinanceFolio.Data;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "./firebaseAuth_sdk.json");
var projectName = (string)Environment.GetEnvironmentVariables()["FirebaseProject"]!;

// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // Example registration for DistributedMemoryCache
builder.Services.AddSession();

// FIREBASE INJECTIONS
builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddSingleton<FirebaseAuthClient>();
builder.Services.AddSingleton<FirebaseAuthConfig>();
builder.Services.AddSingleton<Authentication>();

builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
{
    ApiKey = "AIzaSyAEaM8Ccz1irTF3rjrqJGU65YPozgwb--E",
    AuthDomain = $"authfinancefolio.firebaseapp.com",
    Providers = new FirebaseAuthProvider[]
    {
        new EmailProvider(),
        new GoogleProvider()
    }
}));

builder.Services.AddSingleton<IAuthentication, Authentication>(); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/authfinancefolio";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/authfinancefolio",
            ValidateAudience = true,
            ValidAudience = "authfinancefolio",
            ValidateLifetime = true
        };
    });

// DBCONTEXT INJECTION
builder.Services.AddDbContext<FinanceFolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FinanceFolioDb")));

builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSession();
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();