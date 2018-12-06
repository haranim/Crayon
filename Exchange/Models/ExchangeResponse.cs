using System.Collections.Generic;

namespace Exchange.Models
{
    public class ExchangeResponse
    {
        public string EndDate
        {
            get;
            set;
        }
        public string StartDate
        {
            get;
            set;
        }

        public Dictionary<string, Dictionary<string, decimal>> Rates
        {
            get;
            set;
        }
    }
}
