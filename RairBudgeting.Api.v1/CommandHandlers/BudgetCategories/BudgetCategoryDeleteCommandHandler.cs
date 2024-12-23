using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.CommandHandlers.BudgetCategories;
public class BudgetCategoryDeleteCommandHandler : IRequestHandler<BudgetCategoryDeleteCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryDeleteCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<bool> Handle(BudgetCategoryDeleteCommand request, CancellationToken cancellationToken) {
        foreach (var entityID in request.Id) {
            await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().DeleteById(entityID);
        }
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
