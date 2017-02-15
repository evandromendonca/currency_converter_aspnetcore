using System.Collections.Generic;

namespace CurrencyConverter.Models
{
    public class Currency
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public virtual List<CurrencyRate> CurrencyRates { get; set; }

        public virtual List<CurrencyRate> CurrencyReferences { get; set; }
        
        public Currency()
        {
        }
    }
}