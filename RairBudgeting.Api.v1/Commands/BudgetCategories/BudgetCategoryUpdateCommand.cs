using MediatR;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.Commands.BudgetCategories;
public class BudgetCategoryUpdateCommand : IRequest<bool> {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public bool IsDeleted { get; set; } = false;
}
