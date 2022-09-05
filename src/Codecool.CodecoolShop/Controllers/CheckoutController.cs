using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public ProductService ProductService { get; set; }

        public CheckoutController(ILogger<ProductController> logger)
        {
            _logger = logger;
            ProductService = new ProductService(
                ProductDaoMemory.GetInstance(),
                ProductCategoryDaoMemory.GetInstance(),
                SupplierDaoMemory.GetInstance(),
            ShopCart.GetInstance());
        }

        public IActionResult ShowCart()
        {
            var products = ProductService.GetCartProducts();
            ViewBag.TotalPrice = products.Sum(x => x.DefaultPrice);
            return View(products);
        }

        //public IActionResult CheckoutForm()
        //{
        //    var products = ProductService.GetCartProducts();
        //    ViewBag.TotalPrice = products.Sum(x => x.DefaultPrice);
        //    return View(products);
        //}
        [HttpGet]
        public IActionResult CheckoutForm()
        {
            var products = ProductService.GetCartProducts();
            ViewBag.TotalPrice = products.Sum(x => x.DefaultPrice);
            return View(new Checkout());
        }

        [HttpPost]
        public IActionResult ValidationCheckout(Checkout model)
        {
            return (ModelState.IsValid) ? RedirectToAction("PaymentForm", "Checkout") : View("CheckoutForm",new Checkout());
        }

        [HttpGet]
        public IActionResult PaymentForm()
        {
            var products = ProductService.GetCartProducts();
            ViewBag.TotalPrice = products.Sum(x => x.DefaultPrice);
            return View(new Payment());
        }

        [HttpPost]
        public IActionResult ValidationPayment(Payment model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return (ModelState.IsValid) ? RedirectToAction("ThankYouForPurchase", "Checkout") : View("PaymentForm", new Payment());
        }

        public IActionResult ThankYouForPurchase()
        {
            var products = ProductService.GetCartProducts();
            ViewBag.TotalPrice = ProductService.GetCartProducts().Sum(x => x.DefaultPrice);
            ProductService.EmptyCart();
            return View();
        }
    }
}
