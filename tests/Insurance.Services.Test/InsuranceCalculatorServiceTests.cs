using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.ConnectedServices.ProductApi;
using Insurance.Data.Database.SqlServer;
using Insurance.Data.Domain;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Insurance.Services.Test
{
    [TestFixture]
    public class InsuranceCalculatorServiceTests
    {
        private InsuranceCalculatorService _sut;
        private Mock<IProductApiClient> _productApiMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<InsuranceDbContext>()
                .UseInMemoryDatabase(nameof(InsuranceDbContext))
                .Options;

            _productApiMock = new Mock<IProductApiClient>();
            var insuranceDbContext = new InsuranceDbContext(options);
            
            var rangeRulesTestData = new List<InsuranceRangeRule>
            {
                new InsuranceRangeRule { InclusiveMinSalePrice = 500, ExclusiveMaxSalePrice = 2000, InsuranceCost = 1000 },
                new InsuranceRangeRule { InclusiveMinSalePrice = 2000, ExclusiveMaxSalePrice = decimal.MaxValue, InsuranceCost = 2000 }
            };
            
            var productTypeRulesTestData = new List<InsuranceProductTypeRule>
            {
                new InsuranceProductTypeRule { ProductTypeId = 1000, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToProduct },
                new InsuranceProductTypeRule { ProductTypeId = 2000, InsuranceCost = 1000, Type = InsuranceProductTypeRuleType.AppliesToOrder }
            };
            
            insuranceDbContext.InsuranceRangeRules.AddRange(rangeRulesTestData);
            insuranceDbContext.InsuranceProductTypeRules.AddRange(productTypeRulesTestData);
            insuranceDbContext.SaveChanges();
            
            _sut = new InsuranceCalculatorService(_productApiMock.Object, insuranceDbContext);
        }

        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenProductCanNotBeInsured_ThenShouldCalculateInsuranceAsZero()
        {
            //Arrange
            const decimal expectedInsuranceCost = 0;
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 1000, 1));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, false));

            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenProductCanBeInsuredAndNoRulesMatch_ThenShouldCalculateInsuranceAsZero()
        {
            //Arrange
            const decimal expectedInsuranceCost = 0;
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 499, 1));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, true));
                
            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenProductCanBeInsuredAndOnlyRangeRuleMatched_ThenShouldCalculateInsurance()
        {
            //Arrange
            const decimal expectedInsuranceCost = 1000;
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 500, 1));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, true));
                
            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenProductCanBeInsuredAndOnlyProductTypeRuleMatched_ThenShouldCalculateInsurance()
        {
            //Arrange
            const decimal expectedInsuranceCost = 500;
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 499, 1000));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1000)).ReturnsAsync(new ProductTypeDto(1000, default, true));
                
            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenProductCanBeInsuredAndBothRulesMatched_ThenShouldCalculateInsurance()
        {
            //Arrange
            const decimal expectedInsuranceCost = 2500;
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 3000, 1000));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1000)).ReturnsAsync(new ProductTypeDto(1000, default, true));
                
            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateOrderInsurance_GivenProductIds_WhenNoProductCanBeInsured_ThenShouldCalculateInsuranceAsZero()
        {
            //Arrange
            const decimal expectedInsuranceCost = 0;
            
            var orderDto = new OrderDto(new List<OrderItemDto>
            {
                new OrderItemDto(1,1),
                new OrderItemDto(2,2),
                new OrderItemDto(3,3)
            });    
            
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 500, 1));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(2)).ReturnsAsync(new ProductDto(2, default, 2000, 2));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(3)).ReturnsAsync(new ProductDto(3, default, 3000, 3));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, false));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(2)).ReturnsAsync(new ProductTypeDto(2, default, false));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(3)).ReturnsAsync(new ProductTypeDto(3, default, false));
            
            //Act
            decimal insuranceCost = await _sut.CalculateOrderInsuranceAsync(orderDto);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }   
        
        [Test]
        public async Task CalculateOrderInsurance_GivenProductIds_WhenSomeProductsCanBeInsured_ThenShouldCalculateInsurance()
        {
            //Arrange
            const decimal expectedInsuranceCost = 1000;
            
            var orderDto = new OrderDto(new List<OrderItemDto>
            {
                new OrderItemDto(1,1),
                new OrderItemDto(2,2),
                new OrderItemDto(3,3)
            });    
            
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 500, 1));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(2)).ReturnsAsync(new ProductDto(2, default, 1000, 2));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(3)).ReturnsAsync(new ProductDto(3, default, 2000, 3));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, true));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(2)).ReturnsAsync(new ProductTypeDto(2, default, false));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(3)).ReturnsAsync(new ProductTypeDto(3, default, false));
            
            //Act
            decimal insuranceCost = await _sut.CalculateOrderInsuranceAsync(orderDto);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
        
        [Test]
        public async Task CalculateOrderInsurance_GivenProductIds_WhenAllProductsCanBeInsured_ThenShouldCalculateInsurance()
        {
           //Arrange
            const decimal expectedInsuranceCost = 23000;
            
            var orderDto = new OrderDto(new List<OrderItemDto>
            {
                new OrderItemDto(1,1),
                new OrderItemDto(2,2),
                new OrderItemDto(3,3),
                new OrderItemDto(4,4),
                new OrderItemDto(5,5),
            });    
            
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 499, 1)); //0 x 1
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(2)).ReturnsAsync(new ProductDto(2, default, 500, 2)); //1000 x 2
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(3)).ReturnsAsync(new ProductDto(3, default, 1999, 3)); //1000 x 3
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(4)).ReturnsAsync(new ProductDto(4, default, 2000, 4)); //2000 x 4
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(5)).ReturnsAsync(new ProductDto(5, default, 3000, 5)); //2000 x 5
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1)).ReturnsAsync(new ProductTypeDto(1, default, true));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(2)).ReturnsAsync(new ProductTypeDto(2, default, true));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(3)).ReturnsAsync(new ProductTypeDto(3, default, true));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(4)).ReturnsAsync(new ProductTypeDto(4, default, true));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(5)).ReturnsAsync(new ProductTypeDto(5, default, true));
            
            //Act
            decimal insuranceCost = await _sut.CalculateOrderInsuranceAsync(orderDto);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
    }
}