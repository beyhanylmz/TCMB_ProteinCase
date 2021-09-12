using System.Collections.Generic;
using System.Threading.Tasks;
using ProteinCase.Entities;

namespace ProteinCase.Infrastructure
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> SaveDataFromWebService();
        Task<IEnumerable<Currency>> GetDataWithFilteringAndSorting(Enum.SortingEnum sort, string searchCode);
        Task<IEnumerable<Currency>> GetHistoricalDataWithCurrentCode(string code);
    }
}