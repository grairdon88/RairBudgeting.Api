using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RairBudgeting.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20241221_BudgetLineUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "BudgetLine",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                schema: "dbo",
                table: "BudgetLine",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "BudgetLine");

            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                schema: "dbo",
                table: "BudgetLine");
        }
    }
}
