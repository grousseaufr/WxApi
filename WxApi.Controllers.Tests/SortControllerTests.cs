using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxApi.Controllers;
using WxApi.Dtos.Products;
using WxApi.Services;
using WxApi.Services.Enums;

namespace Tests
{
    public class SortControllerTests
    {
        private List<Product> fakeProducts;

        [SetUp]
        public void Setup()
        {
            fakeProducts =  new List<Product>() {
                                    new Product { Name = "Product A", Price = 99.99M, Quantity = 0 },
                                    new Product { Name = "Product B", Price = 101.99M, Quantity = 0 },
                                    new Product { Name = "Product C", Price = 10.99M, Quantity = 0 },
                                    new Product { Name = "Product D", Price = 5, Quantity = 0 },
                                    new Product { Name = "Product F", Price = 999999999999, Quantity = 0 }};
        }

        [Test]
        public async Task SortController_ReturnsBadRequestResultWhithoutSortOption()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetAllSorted(SortOptions.Ascending)).Returns(Task.Run(() => fakeProducts));

            //Act
            var controller = new SortController(mockProductService.Object);

            //Assert
            var result = await controller.Get(string.Empty);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        [TestCase("Low")]
        [TestCase("High")]
        [TestCase("Ascending")]
        [TestCase("Descending")]
        [TestCase("Recommended")]
        public async Task SortController_ReturnsOkResultWhenValidSortOption(string sortOption)
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
        
            //Act
            var controller = new SortController(mockProductService.Object);

            //Assert
            var result = await controller.Get(sortOption);
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
        }

        [Test]
        [TestCase("aaaa")]
        [TestCase("bbbb")]
        public async Task SortController_ReturnsBadRequestResultWhenInValidSortOption(string sortOption)
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();

            //Act
            var controller = new SortController(mockProductService.Object);

            //Assert
            var result = await controller.Get(sortOption);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }
    }
}