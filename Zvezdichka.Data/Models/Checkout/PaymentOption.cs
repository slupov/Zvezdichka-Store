using System.ComponentModel.DataAnnotations;

namespace Zvezdichka.Data.Models.Checkout
{
    public enum PaymentOptions
    {
        DEBIT_CARD = 0,
        PAY_PAL,
        ON_DELIVERY
    }
}