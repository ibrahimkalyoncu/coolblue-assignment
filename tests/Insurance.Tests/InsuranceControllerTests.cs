using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Services;
using Moq;
using NUnit.Framework;

namespace Insurance.Tests
{
    [TestFixture]
    public class InsuranceControllerTests
    {
        private Mock<IInsuranceCalculatorService> _insuranceCalculatorServiceMock;
        private HomeController _sut;

        [SetUp]
        public void Setup()
        {
            _insuranceCalculatorServiceMock = new Mock<IInsuranceCalculatorService>();
            _sut = new HomeController(_insuranceCalculatorServiceMock.Object);
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
    }
}