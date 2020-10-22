using System.Threading.Tasks;
using Insurance.ConnectedServices.ProductApi;
using Insurance.Data.Database.SqlServer;
using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Services
{
    public class InsuranceCalculatorService : IInsuranceCalculatorService
    {
        private readonly IProductApiClient _productApiClient;
        private readonly InsuranceDbContext _dbContext;

        public InsuranceCalculatorService(
            IProductApiClient productApiClient,
            InsuranceDbContext dbContext)
        {
            _productApiClient = productApiClient;
            _dbContext = dbContext;
        }
        
        public async Task<decimal> CalculateProductInsuranceAsync(int productId)
        {
            var product = await _productApiClient.GetProductByIdAsync(productId);
            var productType = await _productApiClient.GetProductTypeByIdAsync(product.ProductTypeId);
            
            if (productType.CanBeInsured == false)
                return 0;

            var insuranceCost = 0M;

            var insuranceRangeRule = await _dbContext
                .InsuranceRangeRules
                .FirstOrDefaultAsync(rule => rule.InclusiveMinSalePrice <= product.SalesPrice && rule.ExclusiveMaxSalePrice > product.SalesPrice);

            var insuranceProductTypeRule = await _dbContext
                .InsuranceProductTypeRules
                .FirstOrDefaultAsync(rule => rule.ProductTypeId == productType.Id && rule.Type == InsuranceProductTypeRuleType.AppliesToProduct);

            if (insuranceRangeRule != null)
                insuranceCost += insuranceRangeRule.InsuranceCost;

            if (insuranceProductTypeRule != null)
                insuranceCost += insuranceProductTypeRule.InsuranceCost;

            return insuranceCost;
        }
    }
}