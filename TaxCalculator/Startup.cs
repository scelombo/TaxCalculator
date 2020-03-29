using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TaxCalculator.Interfaces;
using TaxCalculator.Services;
using TaxCalculator.Utilities;

namespace TaxCalculator
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }



    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {



      Log.Information("Configurering database context");
      services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


      Log.Information("Configurering dependency injection");


      services.AddSingleton(Log.Logger);

      services.AddSingleton<ICalculationTypeService, CalculationTypeService>();
      services.AddSingleton<IRegionService, RegionService>();
      services.AddSingleton<IRegionTaxService, RegionTaxService>();
      //services.AddSingleton<ICalculationService, CalculationService>();

      services.AddControllersWithViews();
      services.AddRazorPages()
              .AddRazorRuntimeCompilation();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSerilogRequestLogging();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
      });

    }
  }
}
