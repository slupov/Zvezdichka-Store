namespace Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets
{
    public class AppKeyConfig
    {
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }

        public string DropboxAppKey { get; set; }
        public string DropboxAppSecret { get; set; }
        public string DropboxAppAccessToken { get; set; }

        public string CloudinaryAppKey { get; set; }
        public string CloudinaryAppSecret { get; set; }
        public string CloudinaryEnvironmentVariable { get; set; }
        public string CloudinaryName { get; set; }
    }
}
