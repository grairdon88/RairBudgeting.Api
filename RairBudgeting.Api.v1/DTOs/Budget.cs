using RairBudgeting.Api.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs;
public class Budget {
    public int Id { get; set; }
    public DateTime BudgetTime { get; set; }
    public IEnumerable<BudgetLine> Lines { get; set; }
    public decimal Amount { get; set; }

    public bool IsDeleted { get; set; }
}
