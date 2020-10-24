using System.Collections.Generic;
using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Database.SqlServer.Mappings
{
    public class InsuranceRangeRuleMapping : IEntityTypeConfiguration<InsuranceRangeRule>
    {
        public void Configure(EntityTypeBuilder<InsuranceRangeRule> builder)
        {
            builder.HasKey(rule => rule.InsuranceRangeRuleId);
            builder.Property(rule => rule.InsuranceCost).HasPrecision(9, 2);    
            builder.Property(rule => rule.InclusiveMinSalePrice).HasPrecision(9, 2);    
            builder.Property(rule => rule.ExclusiveMaxSalePrice).HasPrecision(9, 2);    

            //This is not for live scenarios. Just to make it easier for this test case  
            builder.HasData(new List<InsuranceRangeRule>
            {
                new InsuranceRangeRule { InsuranceRangeRuleId = 1, InclusiveMinSalePrice = 500, ExclusiveMaxSalePrice = 2000, InsuranceCost = 1000},
                new InsuranceRangeRule { InsuranceRangeRuleId = 2, InclusiveMinSalePrice = 2000, ExclusiveMaxSalePrice = 9999999.99M, InsuranceCost = 1000}
            });
        }
    }
}