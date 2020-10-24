using System.Threading.Tasks;
using Insurance.Domain.Models;

namespace Insurance.Services
{
    public interface IInsuranceCalculatorService
    {
        Task<decimal> CalculateProductInsuranceAsync(int productId);
        Task<decimal> CalculateOrderInsuranceAsync(OrderDto order);
    }
}