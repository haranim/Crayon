using System.ComponentModel.DataAnnotations;

namespace Exchange.Models
{
    public class ExchangeRequest
    {
        [Required]
        public string Currency
        {
            get;
            set;
        }

        [Required]
        public string Dates
        {
            get;
            set;
        }
    }
}