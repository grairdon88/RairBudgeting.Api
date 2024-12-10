using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities;
public class BudgetCategory : Entity, IBudgetCategory {
    public string Name { get; set; }
    public string Description { get; set; }
}
