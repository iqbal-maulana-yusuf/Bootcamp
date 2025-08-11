using Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Json;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.File(new JsonFormatter(), "Logs/log-.json", rollingInterval: RollingInterval.Day) // log ke file per hari                                                                                      // .WriteTo.Console()
    .CreateLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    // builder.Host.UseSerilog();
    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // {
    //     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // });
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=products.db"));


    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}

finally
{
    Log.CloseAndFlush();
}
