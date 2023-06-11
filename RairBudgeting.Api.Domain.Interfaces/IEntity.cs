using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Domain.Interfaces;
public interface IEntity {
    Guid Id { get; set; }
    string UserId { get; set; }
}
