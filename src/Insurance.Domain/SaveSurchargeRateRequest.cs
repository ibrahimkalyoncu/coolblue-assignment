namespace Insurance.Domain
{
    public class SaveSurchargeRateRequest
    {
        public SaveSurchargeRateRequest(int productTypeId, decimal insuranceCost)
        {
            ProductTypeId = productTypeId;
            InsuranceCost = insuranceCost;
        }
        
        public int ProductTypeId { get; }
        public decimal InsuranceCost { get; }
    }
}