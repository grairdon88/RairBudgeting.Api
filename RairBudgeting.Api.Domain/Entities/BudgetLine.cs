using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities;
public class BudgetLine : Entity, IBudgetLine {
    public string Name { get; set; }
    public Guid BudgetCategoryId { get; set; }
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; } = false;

    public Guid BudgetId { get; set; }
}
