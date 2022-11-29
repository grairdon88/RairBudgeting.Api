using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RairBudgeting.Api.Infrastructure.Migrations
{
    public partial class _20221122BudgetLineAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "BudgetLine",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "BudgetLine");
        }
    }
}
