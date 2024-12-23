namespace RairBudgeting.Api.Domain.Entities;
public class BudgetLine : Entity {
    public required string Name { get; set; }
    public int BudgetCategoryId { get; set; }
    public required BudgetCategory BudgetCategory { get; set; }
    public int BudgetId { get; set; }

    public required Budget Budget { get; set; }
    public decimal Amount { get; set; } = 0.0m;
    public decimal PaymentAmount { get; set; } = 0.0m;
}
