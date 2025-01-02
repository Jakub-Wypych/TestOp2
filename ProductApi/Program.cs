using Microsoft.EntityFrameworkCore;
using ProductApi;

namespace ProductApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Poprawnie konfigurujemy DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Opcjonalnie: Wersja z hardcodowanym connection stringiem (np. dla test�w)
        // builder.Services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlite("Data Source=products.db"));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorApp",
                policy =>
                {
                    policy.WithOrigins("https://localhost:7086")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowBlazorApp");
        app.MapControllers();

        app.Run();
    }
}
