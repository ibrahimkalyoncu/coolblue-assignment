using System.Threading.Tasks;

namespace Insurance.Services
{
    public interface IInsuranceCalculatorService
    {
        Task<decimal> CalculateProductInsuranceAsync(int productId);
    }
}