using Gotur.Data.Repository.IRepository;
using Gotur.Models;
using Gotur.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gotur.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        //Listeleme
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll();

            return View(productList);

        }

        //Ürün oluşturma ve güncelleme
        public IActionResult Crup(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    l => new SelectListItem
                    {
                        Text = l.Name,
                        Value = l.Id.ToString()
                    })

            };
            if (id == null || id <= 0)
            {
                return View(productVM);
            }

            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (productVM.Product == null)
            {
                return View(productVM);
            }

            return View(productVM);
        }
        [HttpPost]
        public IActionResult Crup(ProductVM productVM, IFormFile file)
        {
            //Image
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (file != null)
            {
                //Ayni isimli dosyaların yüklenebilmesi
                string fileName = Guid.NewGuid().ToString();

                var uploadRoot = Path.Combine(wwwRootPath, @"img\products");
                var extension = Path.GetExtension(file.FileName);

                //Fotoğraf güncelleme
                if (productVM.Product.Image != null)
                {
                    var oldPicPath = Path.Combine(wwwRootPath, productVM.Product.Image);
                    if (System.IO.File.Exists(oldPicPath))
                    {
                        System.IO.File.Delete(oldPicPath);
                    }

                }

                //Yeni Fotoğraf Yükleme
                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extension),
                           FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.Image = @"\img\products\" + fileName + extension;


            }

            if (productVM.Product.Id <= 0)
            {
                _unitOfWork.Product.Add(productVM.Product);

            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");


        }

        //Ürün Silme
        public IActionResult Delete(int id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}

