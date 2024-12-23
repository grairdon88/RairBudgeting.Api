using AutoMapper;
using MediatR;
using RairBudgeting.Api.Domain.Specifications;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands.BudgetLines;
using RairBudgeting.Api.v1.Commands.Budgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.CommandHandlers.BudgetLines;
public class MarkBudgetLinesAsPaidCommandHandler : IRequestHandler<MarkBudgetLineAsPaidCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MarkBudgetLinesAsPaidCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(MarkBudgetLineAsPaidCommand request, CancellationToken cancellationToken) {
        var includedProperties = new List<string> { "Lines" };
        var findResults = await _unitOfWork.Repository<Domain.Entities.Budget>().Find(new BudgetWithLinesSpecification(request.BudgetId, includedProperties));
        var budget = findResults.FirstOrDefault();

        if(budget == null) {
            return false;
        }

        var budgetLineIdsSet = new HashSet<int>(request.BudgetLineIds);

        foreach(var budgetLine in budget.Lines) {
            if(budgetLineIdsSet.Contains(budgetLine.Id)) {
                //  Mark the budget line as paid
                budgetLine.PaymentAmount = budgetLine.Amount;

                //  Get the budget category for the line
                var budgetCategory = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().GetById(budgetLine.BudgetCategoryId);
                budgetLine.BudgetCategory = budgetCategory;

                //  Update the budget line
                await _unitOfWork.Repository<Domain.Entities.BudgetLine>().Update(budgetLine);
            }
        }

        await _unitOfWork.CompleteAsync();
        return true;
    }
}
