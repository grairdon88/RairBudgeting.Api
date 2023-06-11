using MediatR;

namespace RairBudgeting.Api.v1.DTOs.Commands;

public class DeleteBudgetLineFromBudgetCommand : IRequest<bool> {
    public DeleteBudgetLineFromBudgetCommand() { }

    public DeleteBudgetLineFromBudgetCommand(Guid budgetId, Guid budgetLineId) {
        BudgetId = budgetId;
        BudgetLineId = budgetLineId;
    }

    public Guid BudgetId { get; set; }
    public Guid BudgetLineId { get; set; }
    public string UserId { get; set; }
}