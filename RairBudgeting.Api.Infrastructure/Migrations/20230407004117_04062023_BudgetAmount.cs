using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RairBudgeting.Api.Infrastructure.Migrations
{
    public partial class _04062023_BudgetAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Budget",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "Budget");
        }
    }
}
