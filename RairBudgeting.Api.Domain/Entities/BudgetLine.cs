namespace RairBudgeting.Api.Domain.Entities;
public class BudgetLine : Entity {
    public string Name { get; set; }
    public BudgetCategory BudgetCategory { get; set; }
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; } = false;

    public int BudgetId { get; set; }

    public Budget Budget { get; set; }
}
