using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class AddBudgetLineToBudgetCommand : IRequest<bool> {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public string Name { get; set; }
    public Guid BudgetCategoryId { get; set; }
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; }
    public Guid BudgetId { get; set; }
    public decimal PaymentAmount { get; set; }
}
