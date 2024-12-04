using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RairBudgeting.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAmountToBudget : Migration
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "Budget");
        }
    }
}
