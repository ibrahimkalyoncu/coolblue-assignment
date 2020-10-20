using System.Collections.Generic;
using System.Linq;
using Insurance.Data.Entity;

namespace Insurance.Data
{
    internal class InsuranceRuleRepository : IInsuranceRuleRepository
    {
        //If I would be using a real db and probably EFCore, then I would inject the context on services and not use a repository. 
        //For now, I jut simply use an in memory list
        public IQueryable<InsuranceRangeRule> GetInsuranceSalePriceRangeRules()
        {
            return new List<InsuranceRangeRule>
            {
                new InsuranceRangeRule { InclusiveMinSalePrice = 500, ExclusiveMaxSalePrice = 2000, InsuranceCost = 1000 },
                new InsuranceRangeRule { InclusiveMinSalePrice = 2000, ExclusiveMaxSalePrice = decimal.MaxValue, InsuranceCost = 2000 }
            }.AsQueryable();
        }
        
        public IQueryable<InsuranceProductTypeRule> GetInsuranceProductTypeRules()
        {
            return new List<InsuranceProductTypeRule>
            {
                new InsuranceProductTypeRule { ProductTypeId = 21, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToProduct },
                new InsuranceProductTypeRule { ProductTypeId = 32, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToProduct }
            }.AsQueryable();
        }
    }
}