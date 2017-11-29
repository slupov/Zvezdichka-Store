using System.Collections.Generic;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Services.Contracts.Cloud
{
    public interface ICloudService
    {
        IEnumerable<ImageSource> GetImages(string path);

        void RemoveImage(string path, string fileName);
    }
}
