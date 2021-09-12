using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using ProteinCase.Entities;
using ProteinCase.Infrastructure;
using Xunit;

namespace ProteinCase.Tests
{
    public class CurrencyServiceTests
    {
        [Theory]
        [InlineData(Enum.SortingEnum.Date)]
        [InlineData(Enum.SortingEnum.Name)]
        [InlineData(Enum.SortingEnum.DateDesc)]
        [InlineData(Enum.SortingEnum.NameDesc)]
        public void SortData(Enum.SortingEnum sortingEnum)
        {
            var inMemoryDb = new List<Currency>();
            var inMemorySettings = new Dictionary<string, string>
            {
                {"BaseUriAddress", "https://www.tcmb.gov.tr/kurlar/today.xml"},
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings).Build();

            var mockFactory = new Mock<IHttpClientFactory>();

            var mockRepository = new Mock<IGenericRepository<Currency>>();
            mockRepository.Setup(repository => repository.GetAllAsync())
                .Returns(() => Task.FromResult(inMemoryDb.AsEnumerable()));

            mockRepository.Setup(repository => repository.AddAsync(It.IsAny<Currency>()))
                .Callback((Currency currency) => { inMemoryDb.Add(currency); });

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("today.xml")),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var config = new TypeAdapterConfig();
            config.Scan(typeof(Startup).Assembly);

            var currencyService = new CurrencyService(mockRepository.Object, mockFactory.Object,
                configuration, new Mapper(config));

            currencyService.SaveDataFromWebService().Wait();

            var dataWithFilteringAndSorting =
                currencyService.GetDataWithFilteringAndSorting(sortingEnum, null).Result.ToArray();

            dataWithFilteringAndSorting.Should().HaveCount(21);
            dataWithFilteringAndSorting.Should().BeEquivalentTo(inMemoryDb);
        }

        [Fact]
        public void SaveDataFromWebService_Should_Success()
        {
            var inMemoryDb = new List<Currency>();
            var inMemorySettings = new Dictionary<string, string>
            {
                {"BaseUriAddress", "https://www.tcmb.gov.tr/kurlar/today.xml"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings).Build();

            var mockFactory = new Mock<IHttpClientFactory>();

            var mockRepository = new Mock<IGenericRepository<Currency>>();

            mockRepository.Setup(repository => repository.AddAsync(It.IsAny<Currency>()))
                .Callback((Currency currency) => { inMemoryDb.Add(currency); });

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("today.xml")),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var config = new TypeAdapterConfig();
            config.Scan(typeof(Startup).Assembly);

            var currencyService = new CurrencyService(mockRepository.Object, mockFactory.Object,
                configuration, new Mapper(config));

            var dataFromWebService = currencyService.SaveDataFromWebService().Result;

            var currencies = dataFromWebService as Currency[] ?? dataFromWebService.ToArray();
            currencies.Should().HaveCount(21).And.OnlyHaveUniqueItems();
            currencies.Should().ContainSingle(x => x.CurrencyCode == "USD");
            currencies.Should().OnlyContain(x => x.CurrencyCode.Length == 3);
            inMemoryDb.Should().HaveCount(21);
        }
    }
}