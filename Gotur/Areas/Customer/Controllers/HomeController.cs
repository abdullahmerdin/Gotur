using System.Security.Claims;
using Gotur.Data.Repository.IRepository;
using Gotur.Models;
using Gotur.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotur.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        //Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
            
        }

        // Details ekranina category ile birlikte product bilgilerini gonder //
        public IActionResult Details(int productId)
        {
            Cart cart = new Cart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category"),
            };

            return View(cart);
        }

        // Urunu cart'a ekle //
        [HttpPost]
        public IActionResult Details(Cart cart)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cart.AppUserId = claim.Value;

            Cart cartDb =
                _unitOfWork.Cart.GetFirstOrDefault(p => p.AppUserId == claim.Value && p.ProductId == cart.ProductId);

            //Kaydet - Session sepetteki urun sayisi
                _unitOfWork.Cart.Add(cart);
                _unitOfWork.Save();
            //if (cartDb == null)
            //{
                
            //}
            //else
            //{
            //    //Count artir kaydet
            //}

            return RedirectToAction("Index");
        }
    }
}
