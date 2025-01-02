using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using server.Hubs;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "corsLocalhost";

builder.Services.AddOpenApi();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
                options.AddPolicy(name: CorsPolicy, builder =>
                {
                    builder
                    // .AllowAnyOrigin()  // can't use with allow creds
                    .WithOrigins("http://localhost:8080", "http://localhost:8081", "http://localhost:8082", "http://localhost:5173") // use https on client and these settings get CORS to work
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }));
builder.Services.AddControllers();

var appSettingsSection = builder.Configuration.GetSection(AppSettings.SectionName);
builder.Services.Configure<AppSettings>(appSettingsSection);

// configure jwt authentication
var appSettings = appSettingsSection.Get<AppSettings>();
if (appSettings == null || string.IsNullOrEmpty(appSettings.Secret))
{
    throw new Exception("AppSettings not found in appsettings.json");
}
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection(AppSettings.SectionName))
    .ValidateDataAnnotations() // validate when called
    .ValidateOnStart(); // also on start

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        // added for SignalR per https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/messagehub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<IUserIdProvider, SignalRUserNameProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsPolicy);

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<MessageHub>("/messagehub");

app.Run();

public class AppSettings
{
    public const string SectionName = "AppSettings";

    [Required]
    public required string Secret { get; set; }
}

