using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.Domain.Entities;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class BudgetAddCommandHandler : IRequestHandler<BudgetAddCommand, Budget> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetAddCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Budget> Handle(BudgetAddCommand request, CancellationToken cancellationToken) {
        request.Id = Guid.NewGuid();
        var entityObject = _mapper.Map<Domain.Entities.Budget>(request);
        var createdEntity = await _unitOfWork.Repository<Domain.Entities.Budget>().CreateEntry(entityObject);
        return _mapper.Map<Budget>(createdEntity);
    }
}
