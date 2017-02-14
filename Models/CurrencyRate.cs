using System;

namespace CurrencyConverter.Models
{
    public class CurrencyRate
    {
        public DateTime Date { get; set; }

        public decimal Rate { get; set; }

        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public CurrencyRate()
        {
        }
    }
}
