using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.ViewModels
{
    public class ConvertVM
    {
        [DataType(DataType.Date)]
        [Display(Name = "Select a date, or let it be")]
        public DateTime Date { get; set; }
       
        public List<string> Currencies { get; set; }

        [Display(Name = "First Currency")]
        public string First_currency { get; set; }

        [Display(Name = "Second Currency")]
        public string Second_currency { get; set; }
       
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public ConvertVM()
        {
        }
    }
}