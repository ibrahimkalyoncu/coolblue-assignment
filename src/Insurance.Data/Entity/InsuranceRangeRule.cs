namespace Insurance.Data.Entity
{
    public class InsuranceRangeRule
    {
        //As this is a demo app and should be able to run on any laptop I don't use a real db and an ORM 
        //So there is no need for an id property. It would be defined like below in case. 
        //public int InuranceRangeRuleId { get; set; }

        public decimal InclusiveMinSalePrice { get; set; }
        public decimal ExclusiveMaxSalePrice { get; set; }
        public decimal InsuranceCost { get; set; }
    }
}