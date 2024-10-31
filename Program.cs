using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using RipStainAPI;
using RipStainAPI.Models;
using RipStainAPI.Services;
using static RipStainAPI.SpectreConsoleLoggerProvider;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RipStainDbSettings>(
    builder.Configuration.GetSection("ReportDatabase"));



 builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 104857600; 
    });

// Register services
builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<FoodItemService>();
builder.Services.AddSingleton<VerifiedReportService>();
builder.Services.AddSingleton<UploadService>();

// AUTH
builder.Services.AddAuthentication(options =>
{
    // Setting the default schemes
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // This line ensures sign-in using cookies.
})
.AddCookie(options =>
{
    // You can customize cookie options here if needed
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
})
.AddGoogle(options =>
{
    options.ClientId = "316317116590-l8ne8qpqnkbfrmlshuavhnde3hkiee0q.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google"; // This must match the redirect URI registered in Google
});



//Logger
builder.Logging.ClearProviders(); // Clear other loggers if needed
builder.Logging.AddProvider(new SpectreConsoleLoggerProvider(LogLevel.Information));


// Add Anti-forgery and CORS
builder.Services.AddAntiforgery();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
      policy =>
      {
          policy
            .WithOrigins(
              "http://example.com",
              "http://192.168.100.109:8081",
              "http://localhost:8081",
              "http://localhost:3000",
				"http://localhost:3001",
              "https://localhost:3000",
              "http://localhost:5172")
            .AllowAnyHeader()
            .AllowAnyMethod()
            ;
      });
});




// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline



// Enable Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS for all requests
app.UseCors("AllowAll");
app.UseRouting();
app.UseHttpsRedirection();
app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();



// Enable Anti-forgery
//app.UseAntiforgery();

// Enable HTTPS redirection

// Map Controllers
app.MapControllers();

// Run the application
app.Run();
