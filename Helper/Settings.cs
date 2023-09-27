namespace Dataverse.Multilingual.Feedback.Helper
{
    public interface ISettings
    {
        DataverseConfig DataverseConfig { get; set; }
    }
    public class Settings: ISettings
    {
        public DataverseConfig DataverseConfig { get; set; }
    }

    public class DataverseConfig
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
    }
}
