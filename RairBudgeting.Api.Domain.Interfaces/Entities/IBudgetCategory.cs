using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Domain.Interfaces.Entities;
public interface IBudgetCategory : IEntity {
    string Name { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
}
