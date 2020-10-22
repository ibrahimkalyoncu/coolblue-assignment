namespace Insurance.Data.Domain
{
    public class InsuranceRangeRule
    {
        public int InsuranceRangeRuleId { get; set; }
        public decimal InclusiveMinSalePrice { get; set; }
        public decimal ExclusiveMaxSalePrice { get; set; }
        public decimal InsuranceCost { get; set; }
    }
}