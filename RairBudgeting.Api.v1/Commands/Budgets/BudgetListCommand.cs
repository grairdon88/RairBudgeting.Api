using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.Budgets;
public class BudgetListCommand : IRequest<IEnumerable<Budget>> {
    public bool IncludeDeleted { get; set; } = false;
    public int PageSize { get; set; } = 10;
    public int PageIndex { get; set; } = 0;
    public IEnumerable<string> IncludedProperties { get; set; } = new List<string>();
}
