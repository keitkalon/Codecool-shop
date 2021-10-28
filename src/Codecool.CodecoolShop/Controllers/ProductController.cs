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
using System.Net.Http;
using System.Net;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        public ViewModel mymodel = new ViewModel();
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

        [HttpPost]
        public IActionResult Index()
        {
            
            string category = Request.Form["Category"];
            string supplier = Request.Form["Supplier"];
            mymodel.ProductCategories = ProductService.GetAllCategories().OrderBy(x => x.Name);
            mymodel.Suppliers = ProductService.GetAllSupliers().OrderBy(x => x.Name);
            if (category != null && supplier == "0")
            {
                if (category == "0") mymodel.Products = ProductService.GetAllProducts().OrderBy(x => x.Name);
                else mymodel.Products = ProductService.GetProductsForCategory(Convert.ToInt32(category)).OrderBy(x => x.Name);
                
                return View(mymodel);
            }
            else if (supplier != null && category =="0")
            {
                if (supplier == "0") mymodel.Products = ProductService.GetAllProducts().OrderBy(x => x.Name);
                else mymodel.Products = ProductService.GetProductsForSupplier(Convert.ToInt32(supplier)).OrderBy(x => x.Name);
                
                return View(mymodel);
            }
            else
            {
                var productsCat = ProductService.GetAllProducts().OrderBy(x =>x.Name);
                List<Product> products = new List<Product>();
                foreach (var item in productsCat)
                {
                    if (item.Supplier.Id == Convert.ToInt32(supplier) && item.ProductCategory.Id == Convert.ToInt32(category))
                    {
                        products.Add(item);
                    }
                }
                mymodel.Products = products;
                return View(mymodel);
            }
        }

        public IActionResult Index(int Category = 0)
        {
            mymodel.ProductCategories = ProductService.GetAllCategories().OrderBy(x => x.Name);
            mymodel.Suppliers = ProductService.GetAllSupliers().OrderBy(x => x.Name);
            if (Category == 0)
            {
                mymodel.Products = ProductService.GetAllProducts().OrderBy(x => x.Name);
            }
            else
            {
                mymodel.Products = ProductService.GetProductsForCategory(Category);
            }
            return View(mymodel);
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
    }
}
