using System.Collections.Generic;
using Zvezdichka.Web.Models.Entity;

namespace Zvezdichka.Web.Services
{
    public interface IShoppingCartManager
    {
        void AddToCart(string shoppingCartId, int productId, int quantity);
        void RemoveFromCart(string shoppingCartId, int productId);
        bool HasItem(string shoppingCartId, int productId);

        IEnumerable<CartItem> GetCartItems(string shoppingCartId);
        CartItem GetCartItem(string shoppingCartId, int productId);
    }
}