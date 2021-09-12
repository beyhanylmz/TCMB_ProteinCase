using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProteinCase.Entities;

namespace ProteinCase.Infrastructure
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IGenericRepository<Currency> _currencyRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CurrencyService(IGenericRepository<Currency> currencyRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Currency>> SaveDataFromWebService()
        {
            var data = await Utils.SendHttpClientRequest(_httpClientFactory, _configuration["BaseUriAddress"]);
            var serializer = new XmlSerializer(typeof(CurrencyResponseModel), new XmlRootAttribute("Tarih_Date"));

            var stringReader = new StringReader(data);
            var xmlTextReader = new XmlTextReader(stringReader);

            var response = (CurrencyResponseModel) serializer.Deserialize(xmlTextReader);

            if (response != null)
            {
                var currencies = _mapper.Map<IEnumerable<Currency>>(response.Currencies);
                var now = DateTime.Now;
                foreach (var currency in currencies)
                {
                    currency.CreatedDate = now;
                    await _currencyRepository.AddAsync(currency);
                }

                await _currencyRepository.SaveChanges();
                return currencies;
            }

            return null;
        }

        public async Task<IEnumerable<Currency>> GetDataWithFilteringAndSorting(Enum.SortingEnum sort,
            string searchCode)
        {
            var currencies = await _currencyRepository.GetAllAsync();

            if (currencies != null)
            {
                if (!string.IsNullOrEmpty(searchCode))
                {
                    currencies = currencies.Where(c => c.CurrencyCode.ToUpper() == searchCode.ToUpper());
                }

                switch (sort)
                {
                    case Enum.SortingEnum.Date:
                        currencies = currencies.OrderBy(s => s.CreatedDate);
                        break;
                    case Enum.SortingEnum.DateDesc:
                        currencies = currencies.OrderByDescending(s => s.CreatedDate);
                        break;
                    case Enum.SortingEnum.Name:
                        currencies = currencies.OrderBy(s => s.CurrencyName);
                        break;
                    case Enum.SortingEnum.NameDesc:
                        currencies = currencies.OrderByDescending(s => s.CurrencyName);
                        break;
                    default:
                        currencies = currencies.OrderBy(s => s.CurrencyName);
                        break;
                }
            }

            return currencies;
        }

        public async Task<IEnumerable<Currency>> GetHistoricalDataWithCurrentCode(string code)
        {
            return await _currencyRepository.GetQueryable(x => x.CurrencyCode.ToUpper() == code.ToUpper())
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}