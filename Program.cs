using RipStainAPI.Models;
using RipStainAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RipStainDbSettings>(
    builder.Configuration.GetSection("ReportDatabase"));
/* builder.Services.Configure<FoodItemDbSettings>(
    builder.Configuration.GetSection("ReportDatabase")); */


builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<FoodItemService>();
builder.Services.AddSingleton<VerifiedReportService>();

builder.Services.AddControllers();
builder.Services.AddCors(
    options => {
        options.AddDefaultPolicy(
            builder => {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
            });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();