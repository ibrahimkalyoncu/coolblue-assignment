using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Database.SqlServer.Mappings
{
    internal class InsuranceProductTypeRuleMapping : IEntityTypeConfiguration<InsuranceProductTypeRule>
    {
        public void Configure(EntityTypeBuilder<InsuranceProductTypeRule> builder)
        {
            builder.HasKey(rule => rule.InsuranceProductTypeRuleId);
        }
    }
}