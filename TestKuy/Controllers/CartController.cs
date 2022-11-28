using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TestKuy.Models;

namespace TestKuy.Controllers
{
    public class CartController : Controller
    {
        private readonly ImageDbContext _context;
        private readonly Cart _cart;

        public CartController(ImageDbContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public IActionResult Index()
        {
            var Items = _cart.GetAllCartItems();
            _cart.CartItems = Items;
            return View(_cart);
        }

        public IActionResult AddToCart(string id)
        {
            var selectedVGA = GetVGAById(id);

            if (selectedVGA != null)
            {
                _cart.AddToCart(selectedVGA, quantity: 1);
            }
            return RedirectToAction(actionName: "Index", controllerName:"Image");
        }

        public IActionResult RemoveFromCart(string id)
        {
            var selectedVGA = GetVGAById(id);

            if (selectedVGA != null)
            {
                _cart.RemoveFromCart(selectedVGA);
            }
            return RedirectToAction(actionName: "Index");
        }

        public IActionResult ReduceQuantity(string id)
        {
            var selectedVGA = GetVGAById(id);

            if (selectedVGA != null)
            {
                _cart.ReduceQuantity(selectedVGA);
            }
            return RedirectToAction(actionName: "Index");
        }

        public IActionResult IncreaseQuantity(string id)
        {
            var selectedVGA = GetVGAById(id);

            if (selectedVGA != null)
            {
                _cart.IncreaseQuantity(selectedVGA);
            }
            return RedirectToAction(actionName: "Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction(actionName: "Index");
        }


        public ImageModel GetVGAById(string id)
        {
            return _context.Images.FirstOrDefault(b => b.ProductId == id);
        }
    }
}
