﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs;
public class BudgetCategory {
    public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public bool IsDeleted { get; set; } = false;
}
