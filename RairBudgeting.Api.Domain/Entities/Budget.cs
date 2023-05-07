using RairBudgeting.Api.Domain.Interfaces.Entities;

namespace RairBudgeting.Api.Domain.Entities {
    public class Budget : Entity, IBudget {
        public DateTime BudgetTime { get; set; }
        public List<BudgetLine>? Lines { get; set; }
        public decimal Amount { get; set; }

        public bool IsDeleted { get; set; } = false; 
    }
}