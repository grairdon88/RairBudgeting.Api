using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.Budgets;
public class BudgetAddCommand : IRequest<Budget> {
    public int Id { get; set; }
    public DateTime BudgetTime { get; set; }
    public decimal Amount { get; set; }

    public bool IsDeleted { get; set; } = false;
}
