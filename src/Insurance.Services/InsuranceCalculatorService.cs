using System.Linq;
using System.Threading.Tasks;
using Insurance.ConnectedServices.ProductApi;
using Insurance.Data.Database.SqlServer;
using Insurance.Data.Domain;
using Insurance.Domain;
using Insurance.Domain.Models;
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
                .AsNoTracking()
                .FirstOrDefaultAsync(rule => rule.InclusiveMinSalePrice <= product.SalesPrice && rule.ExclusiveMaxSalePrice > product.SalesPrice);

            var insuranceProductTypeRule = await _dbContext
                .InsuranceProductTypeRules
                .AsNoTracking()
                .FirstOrDefaultAsync(rule => rule.ProductTypeId == productType.Id && rule.Type == InsuranceProductTypeRuleType.AppliesToProduct);

            if (insuranceRangeRule != null)
                insuranceCost += insuranceRangeRule.InsuranceCost;

            if (insuranceProductTypeRule != null)
                insuranceCost += insuranceProductTypeRule.InsuranceCost;

            return insuranceCost;
        }

        public async Task<decimal> CalculateOrderInsuranceAsync(CalculateOrderInsuranceRequest request)
        {
            var productDataTasks = request
                .Items
                .Select(item => item.ProductId)
                .Distinct()
                .Select(productId => _productApiClient.GetProductByIdAsync(productId))
                .ToList();

            await Task.WhenAll(productDataTasks);

            var productTypeTasks = productDataTasks
                .Select(task => task.Result.ProductTypeId)
                .Distinct()
                .Select(productTypeId => _productApiClient.GetProductTypeByIdAsync(productTypeId))
                .ToList();

            await Task.WhenAll(productTypeTasks);

            var rangeRules = await _dbContext
                .InsuranceRangeRules
                .AsNoTracking()
                .ToListAsync();

            var productTypeRules = await _dbContext
                .InsuranceProductTypeRules
                .AsNoTracking()
                .ToListAsync();

            var productDataDictionary = productDataTasks
                .ToDictionary(task => task.Result.Id, task => task.Result);

            var productTypeDictionary = productTypeTasks
                .ToDictionary(task => task.Result.Id, task => task.Result);
            
            var insuranceCost = request
                .Items
                .Sum(CalculateOrderItemInsurance);
            
            return insuranceCost;
            
            decimal CalculateOrderItemInsurance(OrderItemDto item)
            {
                var product = productDataDictionary[item.ProductId];
                var productType = productTypeDictionary[product.ProductTypeId];

                if (!productType.CanBeInsured)
                    return 0;

                var singleItemInsuranceCost = 0M;
                
                var insuranceRangeRule = rangeRules
                    .FirstOrDefault(rule => rule.InclusiveMinSalePrice <= product.SalesPrice && rule.ExclusiveMaxSalePrice > product.SalesPrice);

                var insuranceProductTypeOnProductRule = productTypeRules
                    .SingleOrDefault(rule => rule.ProductTypeId == productType.Id && rule.Type == InsuranceProductTypeRuleType.AppliesToProduct);

                var insuranceProductTypeOnOrderRule = productTypeRules
                    .SingleOrDefault(rule => rule.ProductTypeId == productType.Id && rule.Type == InsuranceProductTypeRuleType.AppliesToOrder);

                if (insuranceRangeRule != null)
                    singleItemInsuranceCost += insuranceRangeRule.InsuranceCost;

                if (insuranceProductTypeOnProductRule != null)
                    singleItemInsuranceCost += insuranceProductTypeOnProductRule.InsuranceCost;

                var totalInsuranceCost = singleItemInsuranceCost * item.Quantity;
                
                if (insuranceProductTypeOnOrderRule != null)
                    totalInsuranceCost += insuranceProductTypeOnOrderRule.InsuranceCost;

                return totalInsuranceCost;
            }
        }

        //Works as upsert. This partially handles conflicts but still there is a small chance of race condition  
        //Using some locks over the transaction (like isolation level serializable) is also possible
        //but may have side effects while reading the data being updating.
        //I think, letting back office throw ex better than affecting the insurance calculation performance as it affects customer experience
        public async Task SaveSurchargeRateAsync(SaveSurchargeRateRequest request)
        {
            var existingRule = await _dbContext
                .InsuranceProductTypeRules
                .FirstOrDefaultAsync(rule => rule.Type == InsuranceProductTypeRuleType.AppliesToProduct 
                                             && rule.ProductTypeId == request.ProductTypeId);

            if (existingRule != null)
            {
                existingRule.InsuranceCost = request.InsuranceCost;
            }
            else
            {
                _dbContext
                    .InsuranceProductTypeRules
                    .Add(new InsuranceProductTypeRule
                    {
                        Type = InsuranceProductTypeRuleType.AppliesToProduct,
                        InsuranceCost = request.InsuranceCost,
                        ProductTypeId = request.ProductTypeId
                    });
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}