using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class DeleteBudgetLineFromBudgetCommandHandler : IRequestHandler<DeleteBudgetLineFromBudgetCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteBudgetLineFromBudgetCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteBudgetLineFromBudgetCommand request, CancellationToken cancellationToken) {
        if(request.BudgetId == Guid.Empty || request.BudgetLineId == Guid.Empty) {
            throw new ArgumentException("BudgetId and BudgetLineName must be provided");
        }

        var budget =  await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);
        
        if(budget == null || budget.Lines == null) {
            throw new ArgumentException("Budget not found");
        }

        var budgetLine = budget.Lines.FirstOrDefault(x => x.Id == request.BudgetLineId);
        budget.Lines.Remove(budgetLine);

        //  Save the budget.
        await _unitOfWork.Repository<Domain.Entities.Budget>().UpdateEntry(budget);

        return true;
    }
}