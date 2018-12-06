using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Exchange.Services;
using Exchange.Models;
using System;

namespace Exchange.Controllers
{
    [Route("Exchange")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public CurrencyController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public IEnumerable<string> Get([FromQuery] ExchangeRequest request)
        {
            string baseCurrency, targetCurrency;
            string errorParsingCurrencyParams = "Expected currency format is BaseCurrencyCode>TargetCurrencyCode (i,e. SEK>NOK)";
            string errorParsingDateParams = "Expected date format is yyyy-MM-dd. Multiple dates should be delimited by comma (i,e. yyyy-MM-dd,yyyy-MM-dd";
            List<string> dates = new List<string>();

            try
            {
                //Parse currency params
                if (request.Currency.Contains('>'))
                {
                    var currency = request.Currency.Split('>');
                    if (currency.Length == 2)
                    {
                        baseCurrency = currency[0];
                        targetCurrency = currency[1];
                    }
                    else
                    {
                        return new List<string> { errorParsingCurrencyParams };
                    }
                }
                else
                {
                    return new List<string> { errorParsingCurrencyParams };
                }
            }
            catch (Exception ex)
            {
                return new List<string> { ex.Message };
            }

            try
            {
                //Parse date params
                if (request.Dates.Contains(','))
                {
                    dates = request.Dates.Split(',').ToList();
                    if (!dates.All(d => d.Length > 0))
                    {
                        return new List<string> { errorParsingDateParams };
                    }
                }
                else
                {
                    var inputDate = DateTime.ParseExact(request.Dates, "yyyy-MM-dd", null);
                    dates.Add(inputDate.ToString("yyyy-MM-dd"));
                }

                //Call service
                var response = _exchangeService.GetMinMaxAvgRateForPeriod(dates, baseCurrency, targetCurrency);

                return response;
            }
            catch (Exception ex)
            {
                return new List<string> { "Unable to process the request due to: ", ex.Message };
            }
        }
    }
}