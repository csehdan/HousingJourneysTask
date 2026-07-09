
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MunicipalityTaxes.Domain.Data;
using MunicipalityTaxes.Domain.Data.Repositories;
using MunicipalityTaxes.Api.Services;

namespace MunicipalityTaxes.Api
{
    public class Program
    {
		public static void Main(string[] args)
		{
			//var builder = WebApplication.CreateBuilder(args);

			//// Add services
			//builder.Services.AddControllers();
			//builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen(c =>
			//{
			//	c.SwaggerDoc("v1", new() { Title = "Tax Management API", Version = "v1" });
			//});

			//// Add Problem Details support (better than custom handler for simple cases)
			//builder.Services.AddProblemDetails();

			//// Configure SQLite Database
			//builder.Services.AddDbContext<TaxContext>(options =>
			//	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

			//// Register service layer
			//builder.Services.AddScoped<ITaxService, TaxService>();

			//var app = builder.Build();

			//// Ensure database exists (includes migrations/seeding)
			//using (var scope = app.Services.CreateScope())
			//{
			//	var db = scope.ServiceProvider.GetRequiredService<TaxContext>();
			//	db.Database.EnsureCreated();
			//}

			//// Middleware pipeline
			//app.UseExceptionHandler();  // No options needed when AddProblemDetails() is used
			//if (app.Environment.IsDevelopment())
			//{
			//	app.UseSwagger();
			//	app.UseSwaggerUI();
			//}

			//app.UseHttpsRedirection();
			//app.MapControllers();

			//app.Run();


			var builder = WebApplication.CreateBuilder(args);

			// Add services
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new() { Title = "Tax Management API", Version = "v1" });
			});

			// Add Problem Details support
			builder.Services.AddProblemDetails();

			// Configure SQLite Database
			builder.Services.AddDbContext<TaxContext>(options =>
				options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Register Repository Pattern (this is the key change!)
			builder.Services.AddScoped<ITaxRepository, TaxRepository>();

			// Register Service Layer
			builder.Services.AddScoped<ITaxService, TaxService>();

			var app = builder.Build();

			// Ensure database exists
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<TaxContext>();
				db.Database.EnsureCreated();
			}

			// Middleware pipeline
			app.UseExceptionHandler();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.MapControllers();

			app.Run();
		}
    }


	public class GlobalExceptionHandler : IExceptionHandler
	{
		public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
		{
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsJsonAsync(new { error = "An internal error occurred", requestId = context.TraceIdentifier }, cancellationToken: cancellationToken);
			return true;
		}
	}
}
