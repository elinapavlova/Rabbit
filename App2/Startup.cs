using Database;
using Infrastructure.Configurations;
using Infrastructure.Repositories.Response;
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
            services.AddDbContextFactory<AppDbContext>(options => options.UseNpgsql(connection,  
                x => x.MigrationsAssembly("Database")));
            
            services.Configure<RabbitMqOptions>(Configuration.GetSection(RabbitMqOptions.App));
            var rabbitMqOptions = Configuration.GetSection(RabbitMqOptions.App).Get<RabbitMqOptions>();
            services.AddSingleton(rabbitMqOptions);

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}