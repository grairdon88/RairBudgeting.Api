using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RairBudgeting.Api.Infrastructure.Migrations
{
    public partial class UpdateDatabaseWithInitialDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLine_Budget_BudgetId",
                schema: "dbo",
                table: "BudgetLine");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                schema: "dbo",
                table: "BudgetLine",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLine_Budget_BudgetId",
                schema: "dbo",
                table: "BudgetLine",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetLine_Budget_BudgetId",
                schema: "dbo",
                table: "BudgetLine");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                schema: "dbo",
                table: "BudgetLine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetLine_Budget_BudgetId",
                schema: "dbo",
                table: "BudgetLine",
                column: "BudgetId",
                principalSchema: "dbo",
                principalTable: "Budget",
                principalColumn: "Id");
        }
    }
}
