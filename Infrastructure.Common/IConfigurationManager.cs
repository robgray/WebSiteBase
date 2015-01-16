namespace Infrastructure.Common
{
    public interface IConfigurationManager
    {
        // Define properties from your config file here.
        string this[string key] { get; }

        string EmailApiKey { get; set; }

        string EmailFrom { get; set; }

        string EmailFromName { get; set; }

        string AdminEmailAddress { get; set; }

        string DeveloperEmailAddress { get; set; }

        bool UseEmailService { get; set; }
    }
}
