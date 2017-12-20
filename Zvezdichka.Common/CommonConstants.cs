namespace Zvezdichka.Common
{
    public class CommonConstants
    {
        public const string WrongStockAmount = "Wrong stock amount provided";

        public const string StockAmountExceededError =
                "You have requested quantities for a product that exceeds our stock level. Please reduce the amount or remove the items and try again."
            ;

        public static string DeletedCartItemSuccessfully = "Successfully removed {0} from your bag!";
        public static string UpdatedCartItemSuccessfully = "Successfully updated your bag!";
    }
}