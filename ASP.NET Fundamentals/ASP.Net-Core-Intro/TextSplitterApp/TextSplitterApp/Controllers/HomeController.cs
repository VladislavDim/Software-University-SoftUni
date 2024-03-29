﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TextSplitterApp.Models;

namespace TextSplitterApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(TextViewModel textViewModel)
        {
            return View(textViewModel);
        }

        public IActionResult Split(TextViewModel textViewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Index", textViewModel);
            }
            var splitTextArray = textViewModel
                .Text
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            textViewModel.SplitText = String.Join(Environment.NewLine, splitTextArray);

            return this.RedirectToAction("Index", textViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}