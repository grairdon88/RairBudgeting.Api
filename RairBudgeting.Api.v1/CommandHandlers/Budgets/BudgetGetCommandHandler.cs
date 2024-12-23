using AutoMapper;
using MediatR;
using RairBudgeting.Api.Domain.Specifications;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using RairBudgeting.Api.v1.Commands.Budgets;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.CommandHandlers.Budgets;
public class BudgetGetCommandHandler : IRequestHandler<BudgetGetCommand, Budget> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetGetCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Budget> Handle(BudgetGetCommand request, CancellationToken cancellationToken) {
        var entities = await _unitOfWork.Repository<Domain.Entities.Budget>().Find(new BudgetWithLinesSpecification(request.Id, request.IncludedEntities));
        var entity = entities.FirstOrDefault();
        return _mapper.Map<Budget>(entity);
    }
}
