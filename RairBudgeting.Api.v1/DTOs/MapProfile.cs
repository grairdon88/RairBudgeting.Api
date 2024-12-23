using AutoMapper;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using RairBudgeting.Api.v1.Commands.BudgetLines;
using RairBudgeting.Api.v1.Commands.Budgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs;
public class MapProfile : Profile {
    public MapProfile() {
        CreateMap<IBudgetCategory, BudgetCategory>();
        CreateMap<BudgetCategory, Domain.Entities.BudgetCategory>();
        CreateMap<INote, Note>();
        CreateMap<Domain.Entities.Budget, Budget>();
        CreateMap<Budget, Domain.Entities.Budget>();
        CreateMap<Domain.Entities.BudgetLine, BudgetLine>();
        CreateMap<BudgetLine, Domain.Entities.BudgetLine>();
        CreateMap<IPayment, Payment>();
        CreateMap<BudgetAddCommand, Domain.Entities.Budget>();
        CreateMap<BudgetCategoryAddCommand, Domain.Entities.BudgetCategory>();
        CreateMap<BudgetUpdateCommand, Domain.Entities.Budget>();
        CreateMap<BudgetCategoryUpdateCommand, Domain.Entities.BudgetCategory>();
        CreateMap<AddBudgetLineToBudgetCommand, Domain.Entities.BudgetLine>();
    }
}
