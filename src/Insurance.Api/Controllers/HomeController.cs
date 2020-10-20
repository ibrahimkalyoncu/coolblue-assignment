using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Models;
using Insurance.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
        private readonly IInsuranceCalculatorService _insuranceCalculatorService;

        public HomeController(IInsuranceCalculatorService insuranceCalculatorService)
        {
            _insuranceCalculatorService = insuranceCalculatorService;
        }
        
        [Obsolete("This endpoint is absolute and will be removed and next major release. Instead please use the GET version of this endpoint")]
        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<InsuranceDto> CalculateProductInsurance([FromBody] InsuranceDto toInsure)
        {
            int productId = toInsure.ProductId;
            toInsure.InsuranceValue = (float)await _insuranceCalculatorService.CalculateProductInsuranceAsync(productId);
            return toInsure;
        }

        [HttpGet]
        [Route("api/insurance/product/{productId}")]
        public async Task<InsuranceCostDto> CalculateProductInsurance([FromRoute] int productId)
        {
            var insuranceCost = await _insuranceCalculatorService.CalculateProductInsuranceAsync(productId);
            return new InsuranceCostDto(insuranceCost);
        }
    }
}