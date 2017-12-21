namespace Zvezdichka.Web.Infrastructure.Constants
{
    public class WebConstants
    {
        public class Routes
        {
            public const string ProductDetails = "product_details";
            public const string ProductEdit = "product_edit";
        }

        public class RoleNames
        {
            public const string AdminRole = "Admin";
            public const string ManagerRole = "Manager";
        }


        public enum OrderBy
        {
            NameAsc,
            NameDesc,
            PriceAsc,
            PriceDesc
        }

        public class Areas
        {
            public const string ProductsArea = "Products";
            public const string ApiArea = "Api";
            public const string AdminArea = "Admin";
            public const string ShoppingArea = "Shopping";
        }
        

        public const decimal TaxPercent = 0.2m;
    }
}