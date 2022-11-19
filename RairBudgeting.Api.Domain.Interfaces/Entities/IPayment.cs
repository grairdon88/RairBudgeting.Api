using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Domain.Interfaces.Entities;
public interface IPayment : IEntity {
    float Amount { get; set; }

    bool IsDeleted { get; set; }
}
