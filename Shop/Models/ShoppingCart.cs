using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public string Id { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { Id = cartId };
        }

        public void AddToCart(Pie pie, int amount)
        {
            var shoppintCartItem = _applicationDbContext.ShoppingCartItems.SingleOrDefault(
                s => s.Pie.Id == pie.Id && s.ShoppingCardId == Id
            );

            if (shoppintCartItem == null)
            {
                shoppintCartItem = new ShoppingCartItem
                {
                    ShoppingCardId = Id,
                    Pie = pie,
                    Amount = 1
                };

                _applicationDbContext.ShoppingCartItems.Add(shoppintCartItem);
            }
            else
            {
                shoppintCartItem.Amount++;
            }

            _applicationDbContext.SaveChanges();
  
        }

        public int RemoveFromCart(Pie pie)
        {
            var shoppingItemCart = _applicationDbContext.ShoppingCartItems.SingleOrDefault(
                     s => s.Pie.Id == pie.Id && s.ShoppingCardId == Id
                );

            var localAmount = 0;

            if(shoppingItemCart != null)
            {
                if(shoppingItemCart.Amount > 1)
                {
                    shoppingItemCart.Amount--;
                    localAmount = shoppingItemCart.Amount;
                }
                else
                {
                    _applicationDbContext.ShoppingCartItems.Remove(shoppingItemCart);
                }
            }

            _applicationDbContext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems(){
            var list = ShoppingCartItems
                ??
                (ShoppingCartItems = _applicationDbContext.ShoppingCartItems.Where(
                    c => c.ShoppingCardId == Id
                    ).Include(s => s.Pie).ToList()
                );

            return list;

        }

        public void ClearCart()
        {
            var cartItems = _applicationDbContext.ShoppingCartItems.Where(
                cart => cart.ShoppingCardId == Id
                );

            _applicationDbContext.ShoppingCartItems.RemoveRange(cartItems);
            _applicationDbContext.SaveChanges();

        }

        public decimal GetShoppingCartTotal()
        {
            var total = _applicationDbContext.ShoppingCartItems.Where(
                cart => cart.ShoppingCardId == Id
                ).Select(c => c.Pie.Price * c.Amount).Sum();
            
            return total;
        }
    }
}
