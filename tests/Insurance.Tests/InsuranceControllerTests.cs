using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Controllers;
using Insurance.Domain;
using Insurance.Domain.Models;
using Insurance.Services;
using Microsoft.AspNetCore.Mvc;
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
            _insuranceCalculatorServiceMock.Verify(x => x.CalculateProductInsuranceAsync(It.IsAny<int>()), Times.Once);
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
            _insuranceCalculatorServiceMock.Verify(x => x.CalculateProductInsuranceAsync(It.IsAny<int>()), Times.Once);
            result.InsuranceCost.Should().Be(expectedCost);
        }
        
        [Test]
        public async Task CalculateOrderInsurance_GivenAnOrder_WhenCalculatorReturnsAResult_ThenShouldReturnInsuranceCost()
        {
            //Arrange
            const decimal expectedCost = 1000M;
            
            _insuranceCalculatorServiceMock
                .Setup(insuranceCalculatorService => insuranceCalculatorService.CalculateOrderInsuranceAsync(It.IsAny<CalculateOrderInsuranceRequest>()))
                .ReturnsAsync(expectedCost);
            
            //Act
            var result = await _sut.CalculateOrderInsurance(new CalculateOrderInsuranceRequest(null));
            
            //Assert
            _insuranceCalculatorServiceMock.Verify(x => x.CalculateOrderInsuranceAsync(It.IsAny<CalculateOrderInsuranceRequest>()), Times.Once);
            result.InsuranceCost.Should().Be(expectedCost);
        }
        
        [Test]
        public async Task SaveSurchargeRate_GivenASurchageRate_WhenSaveSurchargeRateRuns_ThenShouldReturnOk()
        {
            //Arrange
            _insuranceCalculatorServiceMock
                .Setup(insuranceCalculatorService => insuranceCalculatorService.SaveSurchargeRateAsync(It.IsAny<SaveSurchargeRateRequest>()))
                .Verifiable();
            
            //Act
            var result = await _sut.SaveSurchargeRate(new SaveSurchargeRateRequest(1,1));
            
            //Assert
            _insuranceCalculatorServiceMock.Verify(x => x.SaveSurchargeRateAsync(It.IsAny<SaveSurchargeRateRequest>()), Times.Once);
            result.Should().BeAssignableTo<OkResult>();
        }
    }
}