using Gotur.Data.Repository.IRepository;
using Gotur.Models;
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
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products= _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(products);
        }
    }
}
