using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Convert()
        {
            //DateTime data = DateTime.Parse(Request.Form["convert_date"]);
            DateTime data = new DateTime(2017, 02, 02);
            decimal amount = decimal.Parse(Request.Form["amount"]);
            var moeda1 = Request.Form["first_currency"];
            var moeda2 = Request.Form["second_currency"];
            decimal valor1 = 0, valor2 = 0;

            HttpClient hc = new HttpClient();
            string response = hc.GetStringAsync($"http://api.fixer.io/{data.ToString("yyyy-MM-dd")}?base=USD").Result;

            List<string> lista =  response.Remove(response.Length-3).Substring(43).Split(',').ToList();

            foreach(var item in lista)
            {
                var a = item.Split(':');

                if (a.First() == $"\"{moeda1}\"")
                    valor1 = Math.Round(decimal.Parse(a.Last()), 8);

                if (a.First() == $"\"{moeda2}\"")
                    valor2 = Math.Round(decimal.Parse(a.Last()), 8);
            }

            decimal valorFinal = Math.Round((amount/valor1) * valor2, 8);

            ViewBag.valorFinal = Math.Round(valorFinal, 4).ToString();

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.valorFinal = 0.ToString();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            if (!string.IsNullOrWhiteSpace(TempData["valorFinal"].ToString()))
            {
                  ViewBag["result"] = TempData["valorFinal"];
                  //and use you viewbag data in the view
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
