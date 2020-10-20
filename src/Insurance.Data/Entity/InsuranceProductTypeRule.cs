﻿namespace Insurance.Data.Entity
{
    public class InsuranceProductTypeRule
    {
        //public int InsuranceProductTypeRuleId { get; set; }

        public int ProductTypeId { get; set; }
        public decimal InsuranceCost { get; set; }
        public InsuranceProductTypeRuleType Type { get; set; }
    }
}