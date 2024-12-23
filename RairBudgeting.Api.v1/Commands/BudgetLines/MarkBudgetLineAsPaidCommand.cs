using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.BudgetLines;
public class MarkBudgetLineAsPaidCommand : IRequest<bool>{
    public int BudgetId { get; set; }
    public IEnumerable<int> BudgetLineIds { get; set; }
}
