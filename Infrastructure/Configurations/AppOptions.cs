namespace Infrastructure.Configurations
{
    public class AppOptions
    {
        public const string App = "AppOptions";
        public string HostName { get; set; }
        public string QueueTo { get; set; }
        public string QueueFrom { get; set; }
        public string DefaultMessage { get; set; }
    }
}