namespace Insurance.ConnectedServices.ProductApi
{
    public class ProductTypeDto
    {
        public ProductTypeDto(int id, string name, bool canBeInsured)
        {
            Id = id;
            Name = name;
            CanBeInsured = canBeInsured;
        }

        public int Id { get; }
        public string Name { get; }
        public bool CanBeInsured { get; }
    }
}