using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.ConnectedServices.ProductApi;
using Insurance.Data;
using Insurance.Data.Entity;
using Moq;
using NUnit.Framework;

namespace Insurance.Services.Test
{
    [TestFixture]
    public class InsuranceCalculatorServiceTests
    {
        private InsuranceCalculatorService _sut;
        private Mock<IProductApiClient> _productApiMock;
        private Mock<IInsuranceRuleRepository> _insuranceRuleRepository;

        [SetUp]
        public void Setup()
        {
            _productApiMock = new Mock<IProductApiClient>();
            _insuranceRuleRepository = new Mock<IInsuranceRuleRepository>();
            _sut = new InsuranceCalculatorService(_productApiMock.Object, _insuranceRuleRepository.Object);

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
            
            _insuranceRuleRepository.Setup(insuranceRuleRepository => insuranceRuleRepository.GetInsuranceSalePriceRangeRules()).Returns(rangeRulesTestData.AsQueryable());
            _insuranceRuleRepository.Setup(insuranceRuleRepository => insuranceRuleRepository.GetInsuranceProductTypeRules()).Returns(productTypeRulesTestData.AsQueryable());
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
            _productApiMock.Setup(productApiClient => productApiClient.GetProductByIdAsync(1)).ReturnsAsync(new ProductDto(1, default, 2000, 1000));
            _productApiMock.Setup(productApiClient => productApiClient.GetProductTypeByIdAsync(1000)).ReturnsAsync(new ProductTypeDto(1000, default, true));
                
            //Act
            decimal insuranceCost = await _sut.CalculateProductInsuranceAsync(1);

            //Assert
            insuranceCost.Should().Be(expectedInsuranceCost);
        }
    }
}