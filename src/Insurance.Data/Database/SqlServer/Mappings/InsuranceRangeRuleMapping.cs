using Insurance.Data.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Data.Database.SqlServer.Mappings
{
    internal class InsuranceRangeRuleMapping : IEntityTypeConfiguration<InsuranceRangeRule>
    {
        public void Configure(EntityTypeBuilder<InsuranceRangeRule> builder)
        {
            builder.HasKey(rule => rule.InsuranceRangeRuleId);
        }
    }
}