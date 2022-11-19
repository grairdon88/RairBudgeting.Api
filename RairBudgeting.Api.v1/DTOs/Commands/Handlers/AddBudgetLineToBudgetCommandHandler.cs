using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class AddBudgetLineToBudgetCommandHandler : IRequestHandler<AddBudgetLineToBudgetCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddBudgetLineToBudgetCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddBudgetLineToBudgetCommand request, CancellationToken cancellationToken) {
        var entityObject = _mapper.Map<Domain.Entities.BudgetLine>(request);
        var budgetCategory = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().GetById(request.BudgetCategoryId);
        entityObject.BudgetCategory = budgetCategory;

        //var budget = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);
        //if (budget != null) {
        //    budget.Lines.
        //}

        var createdEntity = await _unitOfWork.Repository<Domain.Entities.BudgetLine>().CreateEntry(entityObject);

        return true;

    }
}
