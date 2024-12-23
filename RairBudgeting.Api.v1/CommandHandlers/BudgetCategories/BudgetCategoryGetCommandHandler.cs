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
public class BudgetCategoryGetCommandHandler : IRequestHandler<BudgetCategoryGetCommand, BudgetCategory> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryGetCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BudgetCategory> Handle(BudgetCategoryGetCommand request, CancellationToken cancellationToken) {
        var entity = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().GetById(request.Id);
        return _mapper.Map<BudgetCategory>(entity);
    }
}
