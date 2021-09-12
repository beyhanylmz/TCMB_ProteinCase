using System;
using System.Threading.Tasks;
using ProteinCase.Infrastructure;
using Quartz;

namespace ProteinCase
{
    public class GetExchangeRatesJob : IJob
    {
        private readonly ICurrencyService _currencyService;

        public GetExchangeRatesJob(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _currencyService.SaveDataFromWebService();
            Console.WriteLine("Job Calisti");
        }
    }
}