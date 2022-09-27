using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public ProductService ProductService { get; set; }
        private IProductDao _productDao;

        public ProductController(ILogger<ProductController> logger/*, IProductDao productDao*/)
        {
            //_productDao = productDao;
            _logger = logger;
            SqlManager sqlManager = SqlManager.GetInstance();
            if (!sqlManager.testConnection())
            {
                ProductService = new ProductService(
                    ProductDaoMemory.GetInstance(),
                    ProductCategoryDaoMemory.GetInstance(),
                    SupplierDaoMemory.GetInstance(),
                    ShopCartMemory.GetInstance());
            }
            else
            {
                ProductService = new ProductService(
                    ProductDaoDB.GetInstance(),
                    ProductCategoryDaoDB.GetInstance(),
                    SupplierDaoDB.GetInstance(),
                    ShopCartDB.GetInstance());
            }
        }

        public IActionResult Index()
        {
            ViewBag.Username = MyGlobals.Username;
            //var products = ProductService.GetProductsForCategory(1);
            //return View(products.ToList());
            var categories = ProductService.GetCategories().ToList();
            if (categories.Count > 3)
            {
                List<ProductCategory> categoriesToSend = new List<ProductCategory>();
                Random random = new Random();
                while (categoriesToSend.Count < 3)
                {
                    var index = random.Next(0, categories.Count);
                    categoriesToSend.Add(categories[index]);
                    categories.RemoveAt(index);
                }

                return View(categoriesToSend);
            }
            

            return View(categories);
        }

        public IActionResult Privacy()
        {
            ViewBag.Username = MyGlobals.Username;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.Username = MyGlobals.Username;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Shop()
        {
            ViewBag.Username = MyGlobals.Username;
            ViewBag.Categories = ProductService.GetCategories().OrderBy(x => x.Id).ToList();
            ViewBag.Devs = ProductService.GetSuppliers().OrderBy(x => x.Id).ToList();
            var games = ProductService.GetProducts().ToList();
            return View(games);
        }

        public IActionResult GamesFiltered(int devId, int catId)
        {
            ViewBag.Username = MyGlobals.Username;
            IEnumerable<Product> games = new List<Product>();
            if (devId != 0 && catId != 0)
            {
                games = ProductService.GetByDevAndGenre(devId, catId);
            }
            else if (catId != 0)
            {
                games = ProductService.GetProductsForCategory(catId);
            }
            else if (devId != 0)
            {
                games = ProductService.GetProductBySupplier(devId);
            }
            else
            {
                games = ProductService.GetProducts();
            }

            ViewBag.Categories = ProductService.GetCategories().OrderBy(x => x.Id).ToList();
            ViewBag.Devs = ProductService.GetSuppliers().OrderBy(x => x.Id).ToList();

            ViewBag.catId = catId;
            ViewBag.devId = devId;

            return View(games.ToList());
        }


        public IActionResult AddToCart(int id)
        {
            ViewBag.Username = MyGlobals.Username;
            ProductService.AddToCart(id);
            return RedirectToAction(actionName: "Shop", controllerName: "Product");
        }

        public IActionResult RemoveFromCart(int id)
        {
            ViewBag.Username = MyGlobals.Username;
            ProductService.RemoveFromCart(id);
            return RedirectToAction(controllerName: "Checkout", actionName: "ShowCart");
        }
    }
}
