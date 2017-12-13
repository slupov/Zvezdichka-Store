using CloudinaryDotNet;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Secrets;

namespace Zvezdichka.Web.Infrastructure.Extensions.Cloud
{
    public class CloudinaryExtensions
    {
        public static Cloudinary GetCloudinary(AppKeyConfig appKeys)
        {
            Account account = new Account()
            {
                ApiKey = appKeys.CloudinaryAppKey,
                ApiSecret = appKeys.CloudinaryAppSecret,
                Cloud = appKeys.CloudinaryName
            };

            return new Cloudinary(account);
        }
    }
}