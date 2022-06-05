using System.Security.Claims;
using Gotur.Data.Repository;
using Gotur.Data.Repository.IRepository;
using Gotur.Models;
using Gotur.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotur.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartVM CartVm { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVm = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                OrderProduct = new(),
            };

            foreach (var cart in CartVm.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVm.OrderProduct.OrderPrice += (cart.Price);
            }

            return View(CartVm);
        }

        public IActionResult Order()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVm = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                OrderProduct = new()
            };

            CartVm.OrderProduct.AppUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);
            CartVm.OrderProduct.CellPhone = CartVm.OrderProduct.AppUser.CellPhone;
            CartVm.OrderProduct.Adress = CartVm.OrderProduct.AppUser.Adresses;

            foreach (var cart in CartVm.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVm.OrderProduct.OrderPrice += (cart.Price);
            }

            return View(CartVm);

        }

        [HttpPost]
        [ActionName("Order")]
        [ValidateAntiForgeryToken]
        public IActionResult OrderPost()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVm = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == claim.Value, includeProperties: "Product"),
                OrderProduct = new()
            };
            AppUser appUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == claim.Value);

            CartVm.OrderProduct.AppUser = appUser;
            CartVm.OrderProduct.OrderDate= System.DateTime.Now;
            CartVm.OrderProduct.AppUserId= claim.Value;
            CartVm.OrderProduct.Name=CartVm.OrderProduct.Name;
            CartVm.OrderProduct.CellPhone = CartVm.OrderProduct.AppUser.CellPhone;
            CartVm.OrderProduct.Adress = CartVm.OrderProduct.AppUser.Adresses;
            CartVm.OrderProduct.OrderStatus = "Sipariş Verildi";

            foreach (var cart in CartVm.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                CartVm.OrderProduct.OrderPrice += (cart.Price);
            }

            _unitOfWork.OrderProduct.Add(CartVm.OrderProduct);
            _unitOfWork.Save();

            //Siparis Kalemleri//
            foreach (var cart in CartVm.ListCart)
            {
                OrderDetails OrderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderProductId = CartVm.OrderProduct.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetails.Add(OrderDetails);
                _unitOfWork.Save();
                
            }

            // Order Verildikten Sonra Sepeti Bosaltma //
            List<Cart> Carts = _unitOfWork.Cart.GetAll(u => u.AppUserId == CartVm.OrderProduct.AppUserId).ToList();
            _unitOfWork.Cart.RemoveRange(Carts);
            _unitOfWork.Save();


            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }


        //Urun Sayisi Azaltma Artirma
        public IActionResult Increase(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);

            cart.Count += 1;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Decrease(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);

            if (cart.Count>1)
            {
                cart.Count -= 1;
                _unitOfWork.Save();
            }

            else
            {
                _unitOfWork.Cart.Remove(cart);
                _unitOfWork.Save();

            }

            return RedirectToAction(nameof(Index));
        }
    }

}
