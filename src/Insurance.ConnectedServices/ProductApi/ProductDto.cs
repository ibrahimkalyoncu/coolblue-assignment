namespace Insurance.ConnectedServices.ProductApi
{
    public class ProductDto
    {
        public ProductDto(int id, string name, decimal salesPrice, int productTypeId)
        {
            Id = id;
            Name = name;
            SalesPrice = salesPrice;
            ProductTypeId = productTypeId;
        }
        
        public int Id { get; }
        public string Name { get; }
        public decimal SalesPrice { get; }
        public int ProductTypeId { get; }
    }
}