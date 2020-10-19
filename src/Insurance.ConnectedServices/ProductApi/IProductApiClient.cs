using System.Threading.Tasks;

namespace Insurance.ConnectedServices.ProductApi
{
    public interface IProductApiClient
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<ProductTypeDto> GetProductTypeByIdAsync(int productTypeId);
    }
}