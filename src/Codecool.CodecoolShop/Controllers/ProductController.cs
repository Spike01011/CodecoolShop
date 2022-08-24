using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            ProductService = new ProductService(
                ProductDaoMemory.GetInstance(),
                ProductCategoryDaoMemory.GetInstance(),
                SupplierDaoMemory.GetInstance());
        }

        public IActionResult Index()
        {
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Shop()
        {
            ViewBag.Categories = ProductService.GetCategories().OrderBy(x => x.Id).ToList();
            ViewBag.Devs = ProductService.GetSuppliers().OrderBy(x => x.Id).ToList();
            var games = ProductService.GetProducts().ToList();
            return View(games);
        }

        public IActionResult GamesFiltered(int devId, int catId)
        {
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
    }
}
