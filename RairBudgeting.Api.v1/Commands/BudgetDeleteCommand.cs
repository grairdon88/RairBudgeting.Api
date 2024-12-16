using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands;
public class BudgetDeleteCommand : IRequest<bool> {
    public BudgetDeleteCommand() {
    }

    public BudgetDeleteCommand(int id) {
        BudgetId = id;
    }
    public int BudgetId { get; set; }
}
