using System.Text;
using AutoMapper;
using Common.Exceptions;
using FreelancersApi.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Interfaces;
using Service.Services;
using SmartFreelancerApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON enum support + custom validation error handling
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
	})
	.ConfigureApiBehaviorOptions(options =>
	{
		options.InvalidModelStateResponseFactory = context =>
		{
			var errors = context.ModelState
				.Where(e => e.Value?.Errors.Count > 0)
				.SelectMany(e => e.Value!.Errors.Select(x => x.ErrorMessage));

			throw new BadRequestException(string.Join(" | ", errors));
		};
	});

builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "securityLessonWebApi",
		Version = "v1"
	});

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Enter your token",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey =
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		});

// Database connection
builder.Services.AddDbContext<FreelancerContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("database-home")));

builder.Services.AddScoped<IContext, FreelancerContext>();

// AutoMapper - disable method mapping, load profile
builder.Services.AddAutoMapper(cfg =>
{
	cfg.ShouldMapMethod = (m => false);
	cfg.AddProfile<MapperProfile>();
}, typeof(MapperProfile));

// App services
builder.Services.AddScoped<EmailService>();
builder.Services.AddServices();

// CORS - allow all origins (development only)
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyMethod()
			  .AllowAnyHeader();
	});
});

var app = builder.Build();

// Global error handler
app.UseMiddleware<ErrorHandlingMiddleware>();

// Swagger UI - development only
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowAll");

// Auth middleware - order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();