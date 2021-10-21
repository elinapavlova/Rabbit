using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations
{
    public static class ConfigurationBuilder
    {
        private static readonly IConfigurationRoot Builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath("C:/Users/epavlova/RiderProjects/App1/App1/")
                .AddJsonFile("appsettings.json", false)
                .Build();

        public static IConfigurationRoot Build() 
            => Builder;
    }
}