using voting_system_core.Service.Interface;
using voting_system_core.Service.Impls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using voting_system_core.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IOptionService, OptionService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IDebugService, DebugService>();
builder.Services.AddHttpContextAccessor();

//var secretKey = builder.Configuration["AppSettings:SecretKey"];
//var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Log to console
builder.Logging.AddDebug(); // Log to debug output

builder.Services.AddDbContext<VotingDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var secretKey = builder.Configuration["Appsettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
            ClockSkew = TimeSpan.Zero
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                string authorizationHeader = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    string token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();
app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Logging middleware
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Handling request: {Method} {Path}", context.Request.Method, context.Request.Path);
        await next.Invoke();
        logger.LogInformation("Finished handling request: {StatusCode}", context.Response.StatusCode);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An exception occurred while processing the request: {Method} {Path}", context.Request.Method, context.Request.Path);
        throw;
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
