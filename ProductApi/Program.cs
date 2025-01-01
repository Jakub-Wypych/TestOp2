using Microsoft.EntityFrameworkCore;
using ProductApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        //builder.Services.AddDbContext<AppDbContext>(options =>
        //    options.UseSqlite("DataSource=:memory:"));
        builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlite("Data Source=products.db"));


        builder.Services.AddControllers();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.MapControllers();

        app.Run();
    }
}
