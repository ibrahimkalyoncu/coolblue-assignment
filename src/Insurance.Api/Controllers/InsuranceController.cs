using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Insurance.Domain;
using Insurance.Domain.Models;
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
        [ProducesResponseType(404)]
        public async Task<InsuranceCostDto> CalculateProductInsurance([Range(1,int.MaxValue)][FromRoute] int productId)
        {
            var insuranceCost = await _insuranceCalculatorService.CalculateProductInsuranceAsync(productId);
            return new InsuranceCostDto(insuranceCost);
        }

        [HttpPost]
        [Route("order")]
        public async Task<InsuranceCostDto> CalculateOrderInsurance([FromBody]CalculateOrderInsuranceRequest calculateOrderInsuranceRequest)
        {
            var insuranceCost = await _insuranceCalculatorService.CalculateOrderInsuranceAsync(calculateOrderInsuranceRequest);
            return new InsuranceCostDto(insuranceCost);        
        }
        
        [HttpPut]
        [Route("surcharge")]
        public async Task<IActionResult> SaveSurchargeRate([FromBody]SaveSurchargeRateRequest saveSurchargeRateRequest)
        {
            await _insuranceCalculatorService.SaveSurchargeRateAsync(saveSurchargeRateRequest);
            return Ok();
        }
    }
}