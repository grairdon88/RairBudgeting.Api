using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities;
public class BudgetCategory : Entity, IBudgetCategory {
    public required string Name { get; set; }
    public required string Description { get; set; }
}
