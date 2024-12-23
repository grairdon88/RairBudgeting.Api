using AutoMapper;
using MediatR;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.CommandHandlers.BudgetCategories;
public class BudgetCategoryUpdateCommandHandler : IRequestHandler<BudgetCategoryUpdateCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryUpdateCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<bool> Handle(BudgetCategoryUpdateCommand request, CancellationToken cancellationToken) {
        await _unitOfWork.Repository<BudgetCategory>().Update(_mapper.Map<BudgetCategory>(request));
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
