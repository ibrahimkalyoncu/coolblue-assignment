using Microsoft.EntityFrameworkCore.Migrations;

namespace Insurance.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsuranceProductTypeRules",
                columns: table => new
                {
                    InsuranceProductTypeRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    InsuranceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceProductTypeRules", x => x.InsuranceProductTypeRuleId);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceRangeRules",
                columns: table => new
                {
                    InsuranceRangeRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InclusiveMinSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExclusiveMaxSalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsuranceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceRangeRules", x => x.InsuranceRangeRuleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceProductTypeRules");

            migrationBuilder.DropTable(
                name: "InsuranceRangeRules");
        }
    }
}
