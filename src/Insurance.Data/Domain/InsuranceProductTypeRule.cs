namespace Insurance.Data.Domain
{
    public class InsuranceProductTypeRule
    {
        public int ProductTypeId { get; set; }
        public decimal InsuranceCost { get; set; }
        public InsuranceProductTypeRuleType Type { get; set; }
    }
}