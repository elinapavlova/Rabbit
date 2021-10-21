using Database;
using Infrastructure.Configurations;
using Infrastructure.Repositories.Response;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using ConfigurationBuilder = Infrastructure.Configurations.ConfigurationBuilder;

namespace App2
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = ConfigurationBuilder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection,  
                x => x.MigrationsAssembly("Database")));
            
            services.Configure<AppOptions>(Configuration.GetSection(AppOptions.App));
            var appOptions = Configuration.GetSection(AppOptions.App).Get<AppOptions>();
            services.AddSingleton(appOptions);

            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}