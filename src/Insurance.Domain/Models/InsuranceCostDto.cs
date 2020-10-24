namespace Insurance.Domain.Models
{
    public class InsuranceCostDto
    {
        public InsuranceCostDto(decimal insuranceCost)
        {
            InsuranceCost = insuranceCost;
        }

        public decimal InsuranceCost { get; }
    }
}