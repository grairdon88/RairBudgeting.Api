using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.BudgetCategories;
public class BudgetCategoryGetCommand : IRequest<BudgetCategory> {
    public int Id { get; set; }
}
