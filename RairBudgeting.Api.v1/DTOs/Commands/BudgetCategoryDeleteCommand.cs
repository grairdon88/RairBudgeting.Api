﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class BudgetCategoryDeleteCommand : IRequest<bool> {
    public BudgetCategoryDeleteCommand() {
    }

    public BudgetCategoryDeleteCommand(int id) {
        BudgetCategoryId = id;
    }
    public int BudgetCategoryId { get; set; }
}
