using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.ConnectedServices.ProductApi;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Insurance.ConnectedServices.Test
{
    [TestFixture]
    public class ProductApiClientTests
    {
        private ProductApiClient _client;
        private MockHttpMessageHandler _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new MockHttpMessageHandler();

            var productApiConfigurationMock = new Mock<IProductApiConfiguration>();
            productApiConfigurationMock.SetupGet(productApiConfiguration => productApiConfiguration.BaseAddress).Returns("http://foo");
            
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(_httpMessageHandlerMock.ToHttpClient());

            _client = new ProductApiClient(httpClientFactoryMock.Object, productApiConfigurationMock.Object);
        }

        [Test]
        public async Task GetProductByIdAsync_GivenAProductId_WhenSuccessfulResponse_ThenShouldReturnProductDto()
        {
            //Arrange
            _httpMessageHandlerMock
                .When("/products/1")
                .Respond("application/json",
                    "{'id' : 1, 'name' : 'Test Product 1', 'salesPrice' : 250, 'productTypeId' : 1}");

            //Act
            var product = await _client.GetProductByIdAsync(1);
            
            //Assert
            product.Id.Should().Be(1);
            product.Name.Should().Be("Test Product 1");
            product.SalesPrice.Should().Be(250D);
            product.ProductTypeId.Should().Be(1);
        }
        
        [Test]
        public void GetProductByIdAsync_GivenAProductId_WhenFailedResponse_ThenShouldThrowException()
        {
            //Arrange
            _httpMessageHandlerMock
                .When("/products/1")
                .Respond(HttpStatusCode.NotFound);
            
            //Act
            Func<Task> f = async () => { await _client.GetProductByIdAsync(1); };

            //Assert
            f.Should().Throw<Exception>();
        }
        
        [Test]
        public async Task GetProductTypeByIdAsync_GivenAProductTypeId_WhenSuccessfulResponse_ThenShouldReturnProductType()
        {
            //Arrange
            _httpMessageHandlerMock
                .When("/product_types/1")
                .Respond("application/json",
                    "{'id' : 1, 'name' : 'Test Product Type 1', 'canBeInsured' : true}");
            
            //Act
            var productType = await _client.GetProductTypeByIdAsync(1);

            //Assert
            productType.Id.Should().Be(1);
            productType.Name.Should().Be("Test Product Type 1");
            productType.CanBeInsured.Should().Be(true);
        }
        
        [Test]
        public void GetProductTypeByIdAsync_GivenAProductTypeId_WhenFailedResponse_ThenShouldThrowException()
        {
            //Arrange
            _httpMessageHandlerMock
                .When("/product_types/1")
                .Respond(HttpStatusCode.NotFound);
            
            //Act
            Func<Task> f = async () => { await _client.GetProductByIdAsync(1); };

            //Assert
            f.Should().Throw<Exception>();
        }
    }
}