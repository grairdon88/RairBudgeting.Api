﻿using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.BudgetCategories;
public class BudgetCategoryListCommand : IRequest<IEnumerable<BudgetCategory>> {
    public bool IncludeDeleted { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
