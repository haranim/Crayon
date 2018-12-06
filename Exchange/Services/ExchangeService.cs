using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exchange.Models;
using System.Net;

namespace Exchange.Services
{
    public class ExchangeService : IExchangeService
    {
        public List<string> GetMinMaxAvgRateForPeriod(List<string> dates, string baseCurrency, string targetCurrency)
        {
            try
            {
                var sortedDates = Array.ConvertAll(dates.ToArray(),
                                                   s => DateTime.ParseExact(s, "yyyy-MM-dd", null))
                                                   .OrderBy(d => d);
                
                Dictionary<string, decimal> resultDictionary = GetDateAndRateFromExchangeRatesApi(sortedDates,
                                                                    baseCurrency, targetCurrency);

                var sortedResult = resultDictionary.OrderBy(v => v.Value);

                var minRate = "A min rate of " + sortedResult.First().Value + " on " + sortedResult.First().Key;

                var maxRate = "A max rate of " + sortedResult.Last().Value + " on " + sortedResult.Last().Key;

                var avgRate = "An average rate of " + Math.Round((resultDictionary.Sum(r => r.Value) / sortedDates.Count()), 12, MidpointRounding.AwayFromZero);

                return new List<string> { minRate, maxRate, avgRate };
            }
            catch (WebException e)
            {
                using (var reader = new StreamReader(e.Response.GetResponseStream()))
                {
                    var errorJson = reader.ReadToEnd();

                    return new List<string> { errorJson };
                }
            }
            catch (Exception ex)
            {
                return new List<string> { "Unable to process the request due to: ", ex.Message };
            }
        }

        private Dictionary<string, decimal> GetDateAndRateFromExchangeRatesApi(IOrderedEnumerable<DateTime> sortedDates,
                                             string baseCurrency, string targetCurrency)
        {
            var resultDictionary = new Dictionary<string, decimal>();

            //For now using as hard coded url string.
            var request = WebRequest.Create("https://api.exchangeratesapi.io/history?start_at=" + sortedDates.First().ToString("yyyy-MM-dd") + "&end_at=" + sortedDates.Last().ToString("yyyy-MM-dd") + "&base=" + baseCurrency + "&symbols=" + targetCurrency);
            var response = request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = reader.ReadToEnd();
                var exchangeHistory = JsonConvert.DeserializeObject<ExchangeResponse>(json);

                foreach (var rate in exchangeHistory.Rates)
                {
                    if (sortedDates.Contains(DateTime.Parse(rate.Key)))
                    {
                        var x = rate.Value;
                        foreach (var y in x.Values)
                        {
                            resultDictionary.Add(rate.Key, y);
                        }
                    }
                }
            }
            return resultDictionary;
        }
    }
}