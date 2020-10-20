using System.Linq;
using Insurance.Data.Entity;

namespace Insurance.Data
{
    public interface IInsuranceRuleRepository
    {
        IQueryable<InsuranceRangeRule> GetInsuranceSalePriceRangeRules();
        IQueryable<InsuranceProductTypeRule> GetInsuranceProductTypeRules();
    }
}