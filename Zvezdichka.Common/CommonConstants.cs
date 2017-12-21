namespace Zvezdichka.Common
{
    public class CommonConstants
    {
        public const string WrongStockAmount = "Wrong stock amount provided";

        public const string StockAmountExceededError =
                "You have requested quantities for a product that exceed our stock level. Please reduce the amount or remove the item and try again."
            ;

        public const string StockAmountExceededForError =
                "You have requested quantities for {0} that exceed our stock level. Please reduce the amount or remove the item and try again."
            ;

        public static string DeletedCartItemSuccessfully = "Successfully removed {0} from your bag!";
        public static string UpdatedCartItemSuccessfully = "Successfully updated your bag!";
    }
}