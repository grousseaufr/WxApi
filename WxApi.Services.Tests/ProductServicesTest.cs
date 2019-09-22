using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WxApi.Configuration.Settings;
using WxApi.Dtos.Products;
using WxApi.Dtos.User;
using WxApi.Services;
using WxApi.Services.Enums;

namespace Tests
{
    public class ProductServicesTest
    {
        private readonly string userName = "John Doe";
        private readonly string token = "1111-2222-3333-4444";
        private readonly string jsonDataProducts = "[{'name':'Test Product A', 'price':99.99, 'quantity':0.0},{'name':'Test Product B','price':101.99,'quantity':0.0},{'name':'Test Product C','price':10.99,'quantity':0.0},{'name':'Test Product D','price':5.0,'quantity':0.0},{'name':'Test Product F','price':999999999999.0,'quantity':0.0}]";
        private readonly string jsonDataShopperHistory = "[{'customerId':123,'products':[{'name':'Test Product A','price':99.99,'quantity':3},{'name':'Test Product B','price':101.99,'quantity':1},{'name':'Test Product F','price':999999999999,'quantity':1}]},{'customerId':23,'products':[{'name':'Test Product A','price':99.99,'quantity':2},{'name':'Test Product B','price':101.99,'quantity':3},{'name':'Test Product F','price':999999999999,'quantity':1}]},{'customerId':23,'products':[{'name':'Test Product C','price':10.99,'quantity':2},{'name':'Test Product F','price':999999999999,'quantity':2}]},{'customerId':23,'products':[{'name':'Test Product A','price':99.99,'quantity':1},{'name':'Test Product B','price':101.99,'quantity':1},{'name':'Test Product C','price':10.99,'quantity':1}]}]";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAll_ReturnAllProduct()
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();
            var mockShopperHistoryService = new Mock<IShopperHistoryService>();

            // Mock for HttpClient
            Mock<HttpMessageHandler> handlerMock = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };
            
            //Act
            var productServices = new ProductService(httpClient, mockUserService.Object, mockShopperHistoryService.Object, mockAppSettings.Object);
            var products = await productServices.GetAll();

            //Assert
            Assert.IsInstanceOf(typeof(List<Product>), products);
            Assert.AreEqual(5, products.Count);
        }
        [Test]
        [TestCase(SortOptions.Low)]
        public async Task GetAllSorted_ByLow_ReturnAllProductSortedByPriceFromToLowerToHigher(SortOptions sortOption)
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();
            var mockShopperHistoryService = new Mock<IShopperHistoryService>();

            // Mock for HttpClient
            Mock<HttpMessageHandler> handlerMock = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };

            //Act
            var productServices = new ProductService(httpClient, mockUserService.Object, mockShopperHistoryService.Object, mockAppSettings.Object);
            var products = await productServices.GetAllSorted(sortOption);

            //Assert
            Assert.AreEqual(5, products.Count);
            Assert.IsInstanceOf(typeof(List<Product>), products);

            for (int i = 0; i < products.Count; i++)
            {
                if (i <= products.Count - 2)
                {
                    Assert.LessOrEqual(products[i].Price, products[i + 1].Price);
                }
            }
        }

        [Test]
        [TestCase(SortOptions.High)]
        public async Task GetAllSorted_ByHigh_ReturnAllProductSortedByPriceFromToHigherToLower(SortOptions sortOption)
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();
            var mockShopperHistoryService = new Mock<IShopperHistoryService>();

            // Mock for HttpClient
            Mock<HttpMessageHandler> handlerMock = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };

            //Act
            var productServices = new ProductService(httpClient, mockUserService.Object, mockShopperHistoryService.Object, mockAppSettings.Object);
            var products = await productServices.GetAllSorted(sortOption);

            //Assert
            Assert.AreEqual(5, products.Count);
            for (int i = 0; i < products.Count; i++)
            {
                if (i <= products.Count - 2)
                {
                    Assert.GreaterOrEqual(products[i].Price, products[i + 1].Price);
                }
            }

        }

        [Test]
        [TestCase(SortOptions.Ascending)]
        public async Task GetAllSorted_ByAscending_ReturnAllProductSortedByNameAscending(SortOptions sortOption)
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();
            var mockShopperHistoryService = new Mock<IShopperHistoryService>();

            // Mock for HttpClient
            Mock<HttpMessageHandler> handlerMock = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };

            //Act
            var productServices = new ProductService(httpClient, mockUserService.Object, mockShopperHistoryService.Object, mockAppSettings.Object);
            var products = await productServices.GetAllSorted(sortOption);

            //Assert
            Assert.AreEqual(5, products.Count);
            for (int i = 0; i < products.Count; i++)
            {
                if (i <= products.Count - 2)
                {
                    Assert.LessOrEqual(products[i].Name, products[i + 1].Name);
                }
            }
        }

        [Test]
        [TestCase(SortOptions.Descending)]
        public async Task GetAllSorted_ByDescending_ReturnAllProductSortedByNameDescending(SortOptions sortOption)
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();
            var mockShopperHistoryService = new Mock<IShopperHistoryService>();

            // Mock for HttpClient
            Mock<HttpMessageHandler> handlerMock = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri("http://test.com/") };

            //Act
            var productServices = new ProductService(httpClient, mockUserService.Object, mockShopperHistoryService.Object, mockAppSettings.Object);
            var products = await productServices.GetAllSorted(sortOption);

            //Assert
            Assert.AreEqual(5, products.Count);
            for (int i = 0; i < products.Count; i++)
            {
                if (i <= products.Count - 2)
                {
                    Assert.GreaterOrEqual(products[i].Name, products[i + 1].Name);
                }
            }
        }

        [Test]
        [TestCase(SortOptions.Recommended)]
        public async Task GetAllSorted_ByRecommended_ReturnAllProductSortedByRecommended(SortOptions sortOption)
        {
            //Arrange
            var mockAppSettings = GetMockAppSettings();
            var mockUserService = GetMockUserService();

            // Mock for HttpClient products
            Mock<HttpMessageHandler> handlerMockProduct = GetHttpMessageHandlerMock(jsonDataProducts);
            var httpClientProduct = new HttpClient(handlerMockProduct.Object) { BaseAddress = new Uri("http://test.com/") };

            // Mock for HttpClient shopper history
            Mock<HttpMessageHandler> handlerMockShopperHistory = GetHttpMessageHandlerMock(jsonDataShopperHistory);
            var httpClientShopperHistory = new HttpClient(handlerMockShopperHistory.Object) { BaseAddress = new Uri("http://test.com/") };

            //Act
            var shopperHistoryService = new ShopperHistoryService(httpClientShopperHistory, mockUserService.Object, mockAppSettings.Object);
            var productServices = new ProductService(httpClientProduct, mockUserService.Object, shopperHistoryService, mockAppSettings.Object);
            var products = await productServices.GetAllSorted(sortOption);

            //Assert
            Assert.AreEqual(5, products.Count);
            for (int i = 0; i < products.Count; i++)
            {
                if (i <= products.Count - 2)
                {
                    Assert.GreaterOrEqual(products[i].Quantity, products[i + 1].Quantity);
                }
            }
        }

        private static Mock<IOptions<AppSettings>> GetMockAppSettings()
        {
            //Arrange
            var mockAppSettings = new Mock<IOptions<AppSettings>>();
            mockAppSettings.Setup(s => s.Value).Returns(new AppSettings { WxServiceBaseUrl = "http://test.com" });
            return mockAppSettings;
        }

        private Mock<IUserService> GetMockUserService()
        {
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.Get()).Returns(new User { Name = userName, Token = token });
            return mockUserService;
        }

        private static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(string jsonData)
        {
            // Mock for HttpClient
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(jsonData)
               })
               .Verifiable();

            return handlerMock;
        }
    }
}