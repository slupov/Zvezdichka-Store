using System;

namespace Zvezdichka.Web.Infrastructure.Constants
{
    public class WebConstants
    {
        public class Routes
        {
            public const string ProductDetails = "product_details";
            public const string ProductEdit = "product_edit";
        }

        public enum OrderBy
        {
            NameAsc,
            NameDesc,
            PriceAsc,
            PriceDesc
        }
    }
}
