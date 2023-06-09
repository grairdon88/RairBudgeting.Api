using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class MarkBudgetLinesAsPaidCommand : IRequest<Budget> {
    public Guid BudgetId { get; set; }
    public List<Guid> BudgetLineIds { get; set; }
    public string UserId { get; set; }
}
