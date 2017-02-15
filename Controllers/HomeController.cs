using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.Data;
using CurrencyConverter.Helpers;
using CurrencyConverter.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Convert([Bind("Date, First_currency, Second_currency, Amount")]ConvertVM cvm)
        {   
            string referenceCurrency = "USD";

            // call the converter helper to convert the value
            HelperCurrencyConverter helper = new HelperCurrencyConverter();
            decimal final_value = helper.Convert(cvm.Date, cvm.First_currency, cvm.Second_currency, cvm.Amount, _context,
                referenceCurrency, true, true);

            return Json(new { final_value = final_value});
        }

        [HttpGet]
        public IActionResult Index()
        {
            // select the currencies available in the database
            IEnumerable<string> currencyCodesList = _context.Currencies.Select(o => o.Code).OrderBy(o => o);

            // create a view model to display in the view
            ConvertVM cvm = new ConvertVM();
            cvm.Date = DateTime.Today;
            cvm.Currencies = new List<string>(currencyCodesList);

            return View(cvm);
        }
    }
}
