using AutoMapper;
using MediatR;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class AddBudgetLineToBudgetCommandHandler : IRequestHandler<AddBudgetLineToBudgetCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddBudgetLineToBudgetCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(AddBudgetLineToBudgetCommand request, CancellationToken cancellationToken) {
        var newBudgetLine = _mapper.Map<Domain.Entities.BudgetLine>(request);
        var budget = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);

        if (budget.Lines == null)
            budget.Lines = new List<Domain.Entities.BudgetLine>();

        budget.Lines.Add(newBudgetLine);
        await _unitOfWork.Repository<Domain.Entities.Budget>().UpdateEntry(budget);

        return true;

    }
}
