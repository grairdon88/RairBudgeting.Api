using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.BudgetCategories;
public class BudgetCategoryDeleteCommand : IRequest<bool> {
    public List<int> Id { get; set; }
}
