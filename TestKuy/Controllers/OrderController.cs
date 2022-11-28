using TestKuy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace TestKuy.Controllers
{
    public class OrderController : Controller
    {
        private readonly ImageDbContext _context;
        private readonly Cart _cart;

        public OrderController(ImageDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var cartItems = _cart.GetAllCartItems();
            _cart.CartItems = cartItems;

            if(_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError(key: "", errorMessage: "Cart is empty, Please add VGA first.");
            }

            if (ModelState.IsValid)
            {
                
                CreateOrder(order);
                _cart.ClearCart();
                return View(viewName: "CheckoutComplete", order);
            }
            return View(order);
        }

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            var cartItems = _cart.CartItems;
            var ShipItem = _context.Shippings;
            var mixItem = cartItems.Zip(ShipItem, (c, s) => new { cartItems = c, ShipItem = s });


            foreach (var cs in mixItem)
            {
                var orderitem = new OrderItem()
                {
                    Quantity = cs.cartItems.Quantity,
                    VGAId = cs.cartItems.image.ProductId,
                    OrderId = order.Id,
                    VGAimage = cs.cartItems.image.ImageName,
                    VGAName = cs.cartItems.image.ProductName,
                    Price = cs.cartItems.image.ProductPrice * cs.cartItems.Quantity
                    //UserName = cs.ShipItem.ShipName,
                    //Phone = cs.ShipItem.ShipPhone,
                    //Country = cs.ShipItem.ShipCountry,
                    //State = cs.ShipItem.ShipState,
                    //Zipcode = cs.ShipItem.ShipZipcode
                };
                order.OrderItems.Add(orderitem);
                order.OrderTotal += orderitem.Price;
            }
            
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IActionResult Shipping()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Shipping(Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipping);
                await _context.SaveChangesAsync();
                return RedirectToAction("Checkout");

            }
            return View(shipping);
        }
    }
}
