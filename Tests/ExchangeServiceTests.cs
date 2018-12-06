using NUnit.Framework;
using Exchange.Services;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class ExchangeServiceTests
    {
        private ExchangeService _exchangeService;

        [SetUp]
        public void Setup()
        {
            _exchangeService = new ExchangeService();
        }

        [Test]
        public void GivesErrorWhenDateisInvalid()
        {
            List<string> dates = new List<string>() { "2018", "2017" };
            var response = _exchangeService.GetMinMaxAvgRateForPeriod(dates, "SEK", "NOK");

            Assert.IsTrue(response[1].Contains("not recognized as a valid DateTime"), "Date is Invalid");
        }

        [Test]
        public void GivesErrorWhenCurrencyisInvalid()
        {
            List<string> dates = new List<string>() { "2018-02-01", "2018-02-15" };

            var response = _exchangeService.GetMinMaxAvgRateForPeriod(dates, "se", "no");

            Assert.IsTrue(response[0].Contains("not supported"), "Currency is Invalid");
        }

        [Test]
        public void GivesResultforValidInput()
        {
            List<string> dates = new List<string>() { "2018-02-01","2018-02-15","2018-03-01" };

            var response = _exchangeService.GetMinMaxAvgRateForPeriod(dates, "SEK", "NOK");

            Assert.AreEqual(response.Count, 3 , "Response count");
            Assert.AreEqual(response[0], "A min rate of 0.9546869595 on 2018-03-01", "Minimum rate and date");
            Assert.AreEqual(response[1], "A max rate of 0.9815486993 on 2018-02-15", "Maximum rate and date");
            Assert.AreEqual(response[2], "An average rate of 0.970839476467", "Avg rate and date");
        }
    }
}