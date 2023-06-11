using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class BudgetCloneCommand : IRequest<Budget> {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public DateTime BudgetTime { get; set; }
}
