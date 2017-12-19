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

        public const string ProductsArea = "Products";
        public const string ApiArea = "Api";
        public const string AdminArea = "Admin";
        public const string ShoppingArea = "Shopping";
    }
}
