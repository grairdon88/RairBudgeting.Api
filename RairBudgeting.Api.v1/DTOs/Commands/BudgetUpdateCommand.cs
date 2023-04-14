using MediatR;

namespace RairBudgeting.Api.v1.DTOs.Commands;

public class BudgetUpdateCommand : IRequest<bool> {
    public int Id { get; set; }
    public DateTime BudgetTime { get; set; }
    public decimal Amount { get; set; }

    public bool IsDeleted { get; set; } = false;
}