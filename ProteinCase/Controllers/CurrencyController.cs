using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProteinCase.Infrastructure;

namespace ProteinCase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataWithSortingAndFiltering(Enum.SortingEnum sort, string searchCode)
        {
            var data = await _currencyService.GetDataWithFilteringAndSorting(sort, searchCode);
            return Ok(data);
        }

        [HttpGet("GetHistoricalData")]
        public async Task<IActionResult> GetHistoricalData(string code)
        {
            var data = await _currencyService.GetHistoricalDataWithCurrentCode(code);
            return Ok(data);
        }
    }
}