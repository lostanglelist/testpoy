using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestKuy.Models;


namespace TestKuy.Models
{
    public class Cart
    {
        private readonly ImageDbContext _context;

        public Cart(ImageDbContext context)
        {
            _context = context;
        }
        public string Id { get; set; }
        public List<CartItem> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ImageDbContext>();
            string cartID = session.GetString(key: "Id") ?? Guid.NewGuid().ToString();

            session.SetString(key: "Id", cartID);

            return new Cart(context) { Id = cartID };
        }

        public CartItem GetCartItem(ImageModel image)
        {
            return _context.CartItems.SingleOrDefault(ci =>
            ci.image.ProductId == image.ProductId && ci.CartId == Id);
        }

        public void AddToCart(ImageModel image, float quantity)
        {
            var cartItem = GetCartItem(image);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    image = image,
                    Quantity = quantity,
                    CartId = Id
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }

        public float ReduceQuantity(ImageModel image)
        {
            var cartItem = GetCartItem(image);
            var remainingQuantity = 0;

            if(cartItem != null)
            {
                if(cartItem.Quantity > 1)
                {
                    remainingQuantity = (int)(--cartItem.Quantity);
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public float IncreaseQuantity(ImageModel image)
        {
            var cartItem = GetCartItem(image);
            var remainingQuantity = 0;

            if (cartItem != null)
            {
                if (cartItem.Quantity > 0)
                {
                    remainingQuantity = (int)(++cartItem.Quantity);
                }
                
            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        public void RemoveFromCart(ImageModel image)
        {
            var cartItem = GetCartItem(image);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(ci => ci.CartId == Id);

            _context.CartItems.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public List<CartItem> GetAllCartItems()
        {
            return CartItems ??
                (CartItems = _context.CartItems.Where(ci => ci.CartId == Id).Include(ci => ci.image).ToList());
        }

        public float GetCartTotal()
        {
            return _context.CartItems.Where(ci => ci.CartId == Id).Select(ci => ci.image.ProductPrice * ci.Quantity).Sum();
        }
    }
}
