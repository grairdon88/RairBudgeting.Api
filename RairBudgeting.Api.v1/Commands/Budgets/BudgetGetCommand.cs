using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.Budgets;
public class BudgetGetCommand : IRequest<Budget> {
    public int Id { get; set; }
    public IEnumerable<string> IncludedEntities { get; set; } = new List<string>();
}
