﻿using System.Collections.Generic;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Products.Models
{
    public class ProductEditViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Stock { get; set; }
        public decimal Price { get; set; }
        public ICollection<ImageSource> ImageSources { get; set; }
        public string ThumbnailSource { get; set; }
    }
}