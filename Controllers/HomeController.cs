using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CurrencyConverter.Data;
using CurrencyConverter.Models;
using CurrencyConverter.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyConverter.Controllers
{
    // public class AjaxPostResponse
    // {
    //     public string Message { get; set; }
    //     public string Date { get; set; }
    // }

    public class HomeController : Controller
    {
        // public ActionResult AjaxView()
        // {

        //     DateTime time = DateTime.Now;
        //     return View("AjaxView", time);
        // }

        // [HttpPost]
        // public ActionResult AjaxPost(DateTime date)
        // {
        //     AjaxPostResponse response = new AjaxPostResponse();
        //     response.Date = date.ToString();

        //     if (date <= DateTime.Now)
        //         response.Message = "You entered a date the is less than now!";
        //     else
        //         response.Message = "You entered a date in the future!";

        //     return Json(response);
        // }

        [HttpPost]
        public ActionResult Convert([Bind("Date, First_currency, Second_currency, Amount")]ConvertVM cvm)
        {   
            // declaring variables
            decimal first_rate = 0, second_rate = 0, final_value = 0;

            // check if amount is zero or if its the same currency
            if (cvm.Amount == 0 || cvm.First_currency == cvm.Second_currency)
                return Json(new { final_value = cvm.Amount});

            // check if one of currencies is USD (USD is used as default reference currency)
            if (cvm.First_currency == "USD") 
                first_rate = 1;

            if (cvm.Second_currency == "USD")
                second_rate = 1;

            using (var db = new ApplicationDbContext())
            {
                // first verify if we have the rates in our database
                if (db.CurrenciesRates.Where(item => 
                        item.Currency.Code == cvm.First_currency && item.Date == cvm.Date).Count() == 1)
                    first_rate = db.CurrenciesRates.SingleOrDefault(item => 
                                        item.Currency.Code == cvm.First_currency && item.Date == cvm.Date).Rate;

                if (db.CurrenciesRates.Where(item => 
                        item.Currency.Code == cvm.Second_currency && item.Date == cvm.Date).Count() == 1)
                    second_rate = db.CurrenciesRates.SingleOrDefault(item => 
                                        item.Currency.Code == cvm.Second_currency && item.Date == cvm.Date).Rate;                                                

                // if I did not found the quotes, try to get it online
                if (first_rate == 0 || second_rate == 0)
                {
                    // get the rates from fixer
                    HttpClient hc = new HttpClient();
                    string response = hc.GetStringAsync($"http://api.fixer.io/{cvm.Date.ToString("yyyy-MM-dd")}?base=USD").Result;

                    // get the values of the rates for the selected day
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                                    JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["rates"].ToString());
                    
                    // for each value, make a insert in the database
                    foreach(var value in values)
                    {
                        Currency currency = db.Currencies.SingleOrDefault(o => o.Code == value.Key);
                        decimal rate = 0;

                        // check if the currency exists, if not, add new
                        if (currency == null)
                        {
                            currency = new Currency() { Code = value.Key };
                            db.Currencies.Add(currency);
                        }

                        // check if the rate exists, if not, add new
                        if (!db.CurrenciesRates.Any(o => o.Currency.Code == value.Key && o.Date == cvm.Date))
                        {
                            rate = decimal.Parse(value.Value);
                            db.CurrenciesRates.Add(new CurrencyRate() { Date = cvm.Date, Rate = rate, Currency = currency});
                        }

                        // if these rates are those that we need, store them
                        if (value.Key == cvm.First_currency)
                            first_rate = rate;
                        else if (value.Key == cvm.Second_currency)
                            second_rate = rate;
                    }

                    db.SaveChanges();
                }
            }

            // converting from first to second currency
            final_value = Math.Round((cvm.Amount/first_rate) * second_rate, 4);

            return Json(new { final_value = final_value});
        }

        [HttpGet]
        public IActionResult Index()
        {
            ConvertVM cvm = new ConvertVM();
            cvm.Date = DateTime.Today;
            cvm.Currencies = new List<string>() {"BRL", "USD", "EUR"};

            return View(cvm);
        }

        // public IActionResult About()
        // {
        //     ViewData["Message"] = "Your application description page.";

        //     if (!string.IsNullOrWhiteSpace(TempData["valorFinal"].ToString()))
        //     {
        //           ViewBag["result"] = TempData["valorFinal"];
        //           //and use you viewbag data in the view
        //     }

        //     return View();
        // }

        // public IActionResult Contact()
        // {
        //     ViewData["Message"] = "Your contact page.";

        //     return View();
        // }

        // public IActionResult Error()
        // {
        //     return View();
        // }
    }
}
