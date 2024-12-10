using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities {
    public class Budget : Entity, IBudget {
        public DateTime BudgetTime { get; set; }
        public IEnumerable<BudgetLine>? Lines { get; set; }
        public decimal Amount { get; set; } = decimal.Zero;
    }
}