using System;
using System.Threading.Tasks;
using Insurance.Api.Models;
using Insurance.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController: Controller
    {
        private readonly IInsuranceCalculatorService _insuranceCalculatorService;

        public InsuranceController(IInsuranceCalculatorService insuranceCalculatorService)
        {
            _insuranceCalculatorService = insuranceCalculatorService;
        }
        
        [Obsolete("This endpoint is absolute and will be removed and next major release. Instead please use the GET version of this endpoint")]
        [HttpPost]
        [Route("product")]
        public async Task<InsuranceDto> CalculateProductInsurance([FromBody] InsuranceDto toInsure)
        {
            int productId = toInsure.ProductId;
            toInsure.InsuranceValue = (float)await _insuranceCalculatorService.CalculateProductInsuranceAsync(productId);
            return toInsure;
        }

        [HttpGet]
        [Route("product/{productId}")]
        [ProducesResponseType(500)]
        public async Task<InsuranceCostDto> CalculateProductInsurance([FromRoute] int productId)
        {
            var insuranceCost = await _insuranceCalculatorService.CalculateProductInsuranceAsync(productId);
            return new InsuranceCostDto(insuranceCost);
        }
    }
}