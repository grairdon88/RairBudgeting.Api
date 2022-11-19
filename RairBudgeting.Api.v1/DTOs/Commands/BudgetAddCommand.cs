using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class BudgetAddCommand : IRequest<Budget> {
    public int Id { get; set; }
    public DateTime BudgetTime { get; set; }
    public bool IsDeleted { get; set; } = false;
}
