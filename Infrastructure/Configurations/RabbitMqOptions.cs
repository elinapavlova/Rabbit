namespace Infrastructure.Configurations
{
    public class RabbitMqOptions
    {
        public const string App = "RabbitMqOptions";
        public string HostName { get; set; }
        public string QueueTo { get; set; }
        public string QueueFrom { get; set; }
        public string DefaultMessage { get; set; }
    }
}