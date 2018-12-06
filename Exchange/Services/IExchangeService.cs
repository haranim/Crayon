using System.Collections.Generic;

namespace Exchange.Services
{
    public interface IExchangeService
    {
        List<string> GetMinMaxAvgRateForPeriod(List<string> dates, string baseCurrency, string targetCurrency);
    }
}
