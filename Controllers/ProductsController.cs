using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        private IndexService _index;

        public ProductsController(Data data, IndexService index)
        {
            _index = index;
            myProducts = data.Products;
        }

        [Route("Products/Index")]
        [Route("Items/Index")]
        [Route("Items")]
        [Route("Products")]
        public IActionResult Index(int filterId, string filtername)
        {
            var filtered = myProducts;

            if (!string.IsNullOrEmpty(filtername))
            {
                filtered = myProducts.Where(p => p.Description == filtername).ToList();

                if (filterId > 0)
                {
                    if (filterId == 1)
                    {
                        filtered = filtered.OrderBy(p => p.Id).ToList();
                    }
                    else if (filterId == 2)
                    {
                        filtered = filtered.OrderByDescending(p => p.Id).ToList();
                    }
                }

                return View(filtered);
            }

            return View(myProducts);
        }

        public IActionResult View(int id)
        {
            var filtered = myProducts
                .Where(i => i.Id == id)
                .First();
            return View(filtered);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            _index.Index = myProducts.FindIndex(p => p.Id == id);
            return View(myProducts.Where(i => i.Id == id).First());
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            myProducts[_index.Index].Id = product.Id;
            myProducts[_index.Index].Description = product.Description;
            myProducts[_index.Index].Name = product.Name;
            return View(product);
        }

        [Route("Products/Create")]
        [Route("Products/New")]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            myProducts.Add(product);
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var deletedProduct = myProducts
               .Where(x => x.Id == id)
               .First();

             myProducts.Remove(deletedProduct);
             return View("Index", myProducts);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
