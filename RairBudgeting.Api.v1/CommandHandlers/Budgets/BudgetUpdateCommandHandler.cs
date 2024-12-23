using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.v1.Commands.Budgets;

namespace RairBudgeting.Api.v1.CommandHandlers.Budgets;
public class BudgetUpdateCommandHandler : IRequestHandler<BudgetUpdateCommand, bool> {

    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetUpdateCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(BudgetUpdateCommand request, CancellationToken cancellationToken) {
        var entity = await _unitOfWork.Repository<Budget>().GetById(request.Id);
        entity.BudgetTime = request.BudgetTime;
        entity.IsDeleted = request.IsDeleted;
        await _unitOfWork.Repository<Budget>().Update(entity);

        return true;
    }
}
