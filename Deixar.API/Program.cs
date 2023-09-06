using Deixar.API.Middleware;
using Deixar.API.Swagger;
using Deixar.Data.Contexts;
using Deixar.Data.HealthChecks;
using Deixar.Data.Repositories;
using Deixar.Domain.Interfaces;
using Deixar.Domain.Utilities;
using Deixar.Domain.Validators;
using Deixar.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//Generate Swagger
builder.Services.AddSwaggerGen();

//Setup Application DBContext
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Add health checks
builder.Services.AddHealthChecks().AddCheck<DBConnectionHealthCheck>("Database connection health check.");

//Setup fluent validations
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();

#region Service Registration
EmailConfiguration emailConfig = builder.Configuration.GetSection("MailSettings").Get<EmailConfiguration>()!;
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<IEmailUtility, EmailUtility>();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddTransient<EmailUtility>();
builder.Services.AddTransient<TokenUtility>();
#endregion Service Registration

//Add support for API versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});

//Setup api explorer that is API version aware
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

//JWT Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = builder.Configuration["Jwt:Key"]!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.Zero,
    };
});

//Sentry services
builder.WebHost.UseSentry();

//Serilog services
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

//Map endpoint for health checking
app.MapHealthChecks("/api/health");

app.UseHttpsRedirection();

//Enable sentry tracing
app.UseSentryTracing();

//Enable serilog logging
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();