using AutoMapper;
using Database;
using Infrastructure.AppProfile;
using Infrastructure.Configurations;
using Infrastructure.Repositories.Response;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using ConfigurationBuilder = Infrastructure.Configurations.ConfigurationBuilder;

namespace App1
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

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}