using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class UpdateBudgetLineInBudgetCommandHandler : IRequestHandler<UpdateBudgetLineInBudgetCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateBudgetLineInBudgetCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateBudgetLineInBudgetCommand request, CancellationToken cancellationToken) {
        var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);
        if(entity == null) {
            throw new ArgumentException("Budget not found");
        }

        var budgetLine = entity.Lines.FirstOrDefault(x => x.Id == request.Id);

        if(budgetLine == null) {
            throw new ArgumentException("Budget line not found");
        }
        budgetLine.Amount = request.Amount;
        budgetLine.BudgetCategoryId = request.BudgetCategoryId;
        budgetLine.Name = request.Name;

        await _unitOfWork.Repository<Domain.Entities.Budget>().UpdateEntry(entity);
        return true;
    }
}
