using MediatR;

namespace RairBudgeting.Api.v1.Commands;

public class BudgetUpdateCommand : IRequest<bool> {
    public int Id { get; set; }
    public DateTime BudgetTime { get; set; }
    public bool IsDeleted { get; set; } = false;
}