using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands;
public class AddBudgetLineToBudgetCommand : IRequest<bool> {
    public int Id { get; set; }
    public string Name { get; set; }
    public int BudgetCategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public int BudgetId { get; set; }
}
