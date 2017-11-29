using System.Collections.Generic;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Cloud;

namespace Zvezdichka.Services.Implementations.Cloud
{
    public class DropboxCloudService : ICloudService
    {
        public IEnumerable<ImageSource> GetImages(string path)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveImage(string path, string fileName)
        {
            throw new System.NotImplementedException();
        }
    }
}
