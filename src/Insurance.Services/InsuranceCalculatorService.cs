using System;
using System.Linq;
using System.Threading.Tasks;
using Insurance.ConnectedServices.ProductApi;
using Insurance.Data;
using Insurance.Data.Entity;

namespace Insurance.Services
{
    public class InsuranceCalculatorService : IInsuranceCalculatorService
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IInsuranceRuleRepository _insuranceRuleRepository;

        public InsuranceCalculatorService(
            IProductApiClient productApiClient,
            IInsuranceRuleRepository insuranceRuleRepository)
        {
            _productApiClient = productApiClient;
            _insuranceRuleRepository = insuranceRuleRepository;
        }
        
        public async Task<decimal> CalculateProductInsuranceAsync(int productId)
        {
            var product = await _productApiClient.GetProductByIdAsync(productId);
            var productType = await _productApiClient.GetProductTypeByIdAsync(product.ProductTypeId);
            
            if (productType.CanBeInsured == false)
                return 0;

            var insuranceCost = 0M;

            var insuranceRangeRule = _insuranceRuleRepository
                .GetInsuranceSalePriceRangeRules()
                .FirstOrDefault(rule => rule.InclusiveMinSalePrice <= product.SalesPrice && rule.ExclusiveMaxSalePrice > product.SalesPrice);

            var insuranceProductTypeRule = _insuranceRuleRepository
                .GetInsuranceProductTypeRules()
                .FirstOrDefault(rule => rule.ProductTypeId == productType.Id && rule.Type == InsuranceProductTypeRuleType.AppliesToProduct);

            if (insuranceRangeRule != null)
                insuranceCost += insuranceRangeRule.InsuranceCost;

            if (insuranceProductTypeRule != null)
                insuranceCost += insuranceProductTypeRule.InsuranceCost;

            return insuranceCost;
        }
    }
}