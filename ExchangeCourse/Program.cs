using ExchangeCourse.Abstractions;
using ExchangeCourse.Database;
using ExchangeCourse.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();