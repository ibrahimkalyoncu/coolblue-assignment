using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Domain.Models;
using Insurance.Services;
using Moq;
using NUnit.Framework;

namespace Insurance.Tests
{
    [TestFixture]
    public class InsuranceControllerTests
    {
        private Mock<IInsuranceCalculatorService> _insuranceCalculatorServiceMock;
        private InsuranceController _sut;

        [SetUp]
        public void Setup()
        {
            _insuranceCalculatorServiceMock = new Mock<IInsuranceCalculatorService>();
            _sut = new InsuranceController(_insuranceCalculatorServiceMock.Object);
        }

        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenCalculatorReturnsAResult_ThenShouldReturnInsuranceCost_Obsolete()
        {
            //Arrange
            const decimal expectedInsuranceValue = 1000M;
            
            _insuranceCalculatorServiceMock
                .Setup(insuranceCalculatorService => insuranceCalculatorService.CalculateProductInsuranceAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedInsuranceValue);
            
            //Act
            var result = await _sut.CalculateProductInsurance(new InsuranceDto { ProductId = 1});
            
            //Assert
            result.InsuranceValue.Should().Be((float)expectedInsuranceValue);
        }
        
        [Test]
        public async Task CalculateProductInsurance_GivenAProductId_WhenCalculatorReturnsAResult_ThenShouldReturnInsuranceCost()
        {
            //Arrange
            const decimal expectedCost = 1000M;
            
            _insuranceCalculatorServiceMock
                .Setup(insuranceCalculatorService => insuranceCalculatorService.CalculateProductInsuranceAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedCost);
            
            //Act
            var result = await _sut.CalculateProductInsurance(1);
            
            //Assert
            result.InsuranceCost.Should().Be(expectedCost);
        }
        
        [Test]
        public async Task CalculateOrderInsurance_GivenAnOrder_WhenCalculatorReturnsAResult_ThenShouldReturnInsuranceCost()
        {
            //Arrange
            const decimal expectedCost = 1000M;
            
            _insuranceCalculatorServiceMock
                .Setup(insuranceCalculatorService => insuranceCalculatorService.CalculateOrderInsuranceAsync(It.IsAny<OrderDto>()))
                .ReturnsAsync(expectedCost);
            
            //Act
            var result = await _sut.CalculateOrderInsurance(new OrderDto(null));
            
            //Assert
            result.InsuranceCost.Should().Be(expectedCost);
        }
    }
}