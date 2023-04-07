using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
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
        var entity = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().GetById(request.Id);

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().Update(_mapper.Map<Domain.Entities.BudgetCategory>(request));
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
