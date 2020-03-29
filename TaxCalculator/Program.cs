using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaxCalculator.Utilities;

namespace TaxCalculator
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

      Log.Logger = new LoggerConfiguration()
                  .ReadFrom
                  .Configuration(configuration)
                  .CreateLogger();

      try
      {
        
        Log.Information($"Application starting up");

        var host = CreateHostBuilder(args).Build();

        CreateDbIfNotExists(host);
        RunSeeding(host);

        host.Run();

      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Application chrashed");
      }
      finally
      {
        Log.CloseAndFlush();
      }

    }


    private static void CreateDbIfNotExists(IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<Context>();
        context.Database.EnsureCreated();
      }

    }

    private static void RunSeeding(IHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<Context>();
        DbInitializer.Initialize(context, Log.Logger);
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });

  }

}
