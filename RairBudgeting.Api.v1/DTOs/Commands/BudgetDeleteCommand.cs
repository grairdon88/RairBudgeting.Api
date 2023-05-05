using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class BudgetDeleteCommand : IRequest<bool> {
    public BudgetDeleteCommand() {
    }

    public BudgetDeleteCommand(Guid id) {
        Id = id;
    }
    public Guid Id { get; set; }
}
