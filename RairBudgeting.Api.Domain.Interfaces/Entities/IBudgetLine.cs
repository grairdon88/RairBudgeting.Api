using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Domain.Interfaces.Entities;
public interface IBudgetLine : IEntity {
    string Name { get; set; }

    IBudgetCategory BudgetCategory { get; set; }
    decimal Amount { get; set; }

    bool IsDeleted { get; set; }

}
