using ApiDependencies;
using FinanceFolio.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "./firebaseAuth_sdk.json");
var projectName = Environment.GetEnvironmentVariable("FirebaseProject");

// Add services to the container.

builder.Services.AddApiDependencies();

// DB CONTEXT INJECTION
builder.Services.AddDbContext<FinanceFolioContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FinanceFolioDb")));

builder.Services.AddControllers();

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