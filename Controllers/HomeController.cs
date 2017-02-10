using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Controllers
{
    public class AjaxPostResponse
    {
        public string Message { get; set; }
        public string Date { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult AjaxView()
        {

            DateTime time = DateTime.Now;
            return View("AjaxView", time);
        }

        [HttpPost]
        public ActionResult AjaxPost(DateTime date)
        {
            AjaxPostResponse response = new AjaxPostResponse();
            response.Date = date.ToString();

            if (date <= DateTime.Now)
                response.Message = "You entered a date the is less than now!";
            else
                response.Message = "You entered a date in the future!";

            return Json(response);
        }

        [HttpPost]
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

            var valorFinal = Math.Round((amount/valor1) * valor2, 4);

            return Json(new { valorfinal = valorFinal});
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
