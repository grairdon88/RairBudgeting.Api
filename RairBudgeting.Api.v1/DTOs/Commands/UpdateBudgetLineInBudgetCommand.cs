using MediatR;

namespace RairBudgeting.Api.v1.DTOs.Commands;

public class UpdateBudgetLineInBudgetCommand : IRequest<bool> {
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public Guid BudgetCategoryId { get; set; }
    public decimal Amount { get; set; }
    public Guid BudgetId { get; set; }
    public decimal PaymentAmount { get; set; }
}