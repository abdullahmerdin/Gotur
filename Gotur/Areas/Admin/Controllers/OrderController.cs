using Gotur.Data.Repository.IRepository;
using Gotur.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gotur.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class OrderController : Controller
    {
        //Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVm { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        

        public IActionResult Index()
        {
            //Tamamlanmamış tüm siparişleri getir
            var orderList = _unitOfWork.OrderProduct.GetAll(x=>x.OrderStatus!="Teslim Edildi");
            return View(orderList);
        }
    }
}
