using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class BudgetCategoryAddCommandHandler : IRequestHandler<BudgetCategoryAddCommand, BudgetCategory> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCategoryAddCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BudgetCategory> Handle(BudgetCategoryAddCommand request, CancellationToken cancellationToken) {
        var entityObject = _mapper.Map<Domain.Entities.BudgetCategory>(request);
        var createdEntity = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().CreateEntry(entityObject);
        return _mapper.Map<BudgetCategory>(createdEntity);
    }
}
