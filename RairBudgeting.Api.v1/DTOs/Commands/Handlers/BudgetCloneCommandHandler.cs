using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class BudgetCloneCommandHandler : IRequestHandler<BudgetCloneCommand, Budget> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetCloneCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Budget> Handle(BudgetCloneCommand request, CancellationToken cancellationToken) {
        var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.Id);
        var newBudgetID = Guid.NewGuid();

        var entityObject = new Domain.Entities.Budget {
            Id = newBudgetID,
            BudgetTime = request.BudgetTime,
            Amount = entity.Amount,
            Lines = entity.Lines.Select(x => new Domain.Entities.BudgetLine {
                Id = Guid.NewGuid(),
                Amount = x.Amount,
                BudgetCategoryId = x.BudgetCategoryId,
                Name = x.Name,
                BudgetId = newBudgetID
            }).ToList()
        };
        var createdEntity = await _unitOfWork.Repository<Domain.Entities.Budget>().CreateEntry(entityObject);
        return _mapper.Map<Budget>(createdEntity);
    }
}
