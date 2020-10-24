using System.Collections.Generic;
using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Database.SqlServer.Mappings
{
    public class InsuranceProductTypeRuleMapping : IEntityTypeConfiguration<InsuranceProductTypeRule>
    {
        public void Configure(EntityTypeBuilder<InsuranceProductTypeRule> builder)
        {
            builder.HasKey(rule => new { rule.ProductTypeId, rule.Type });

            //This is not for live scenarios. Just to make it easier for this test case  
            builder.HasData(new List<InsuranceProductTypeRule>
            {
                new InsuranceProductTypeRule { ProductTypeId = 21, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToProduct }, //Laptops for each product
                new InsuranceProductTypeRule { ProductTypeId = 32, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToProduct }, //Smartphones for each product
                new InsuranceProductTypeRule { ProductTypeId = 33, InsuranceCost = 500, Type = InsuranceProductTypeRuleType.AppliesToOrder } // Digital Cameras (Drones) once for each order
            });
        }
    }
}