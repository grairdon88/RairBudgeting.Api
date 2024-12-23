using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.CommandHandlers.BudgetCategories;
public class BudgetCategoryListCommandHandler : IRequestHandler<BudgetCategoryListCommand, IEnumerable<BudgetCategory>> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryListCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IEnumerable<BudgetCategory>> Handle(BudgetCategoryListCommand request, CancellationToken cancellationToken) {
        var entities = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().Get(
            x => x.IsDeleted == false || request.IncludeDeleted == true,
            orderBy: null,
            request.PageSize,
            request.PageIndex);

        return _mapper.Map<IEnumerable<BudgetCategory>>(entities);
    }
}
