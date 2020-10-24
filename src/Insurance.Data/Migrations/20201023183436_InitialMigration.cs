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
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InsuranceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceProductTypeRules", x => new { x.ProductTypeId, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "InsuranceRangeRules",
                columns: table => new
                {
                    InsuranceRangeRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InclusiveMinSalePrice = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    ExclusiveMaxSalePrice = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    InsuranceCost = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceRangeRules", x => x.InsuranceRangeRuleId);
                });

            migrationBuilder.InsertData(
                table: "InsuranceProductTypeRules",
                columns: new[] { "ProductTypeId", "Type", "InsuranceCost" },
                values: new object[,]
                {
                    { 21, 1, 500m },
                    { 32, 1, 500m },
                    { 33, 2, 500m }
                });

            migrationBuilder.InsertData(
                table: "InsuranceRangeRules",
                columns: new[] { "InsuranceRangeRuleId", "ExclusiveMaxSalePrice", "InclusiveMinSalePrice", "InsuranceCost" },
                values: new object[,]
                {
                    { 1, 2000m, 500m, 1000m },
                    { 2, 9999999.99m, 2000m, 1000m }
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
