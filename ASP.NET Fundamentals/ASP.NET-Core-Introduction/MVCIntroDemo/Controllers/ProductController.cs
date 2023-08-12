﻿namespace MVCIntroDemo.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using MVCIntroDemo.ViewModels.Product;
    using Newtonsoft.Json;
    using System.Text.Json;
    using static MVCIntroDemo.Seeding.ProductsData;
    public class ProductController : Controller
    {
        public IActionResult All()
        {
            return View(Products);
        }

        public IActionResult ById(string id)
        {
            ProductViewModel? product = Products
                .FirstOrDefault(p=>p.Id.ToString().Equals(id));

            if(product == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(product);
        }

        public IActionResult AllAsJson()
        {
            return Json(Products,new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }
    }
}
