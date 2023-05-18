using AutoMapper;
using MediatR;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class MarkBudgetLinesAsPaidCommandHandler : IRequestHandler<MarkBudgetLinesAsPaidCommand, Budget> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public MarkBudgetLinesAsPaidCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Budget> Handle(MarkBudgetLinesAsPaidCommand request, CancellationToken cancellationToken) {
        var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);
        if(entity == null) {
            throw new ArgumentException("Budget not found");
        }
        foreach(var budgetLineId in request.BudgetLineIds) {
            var budgetLine = entity.Lines.FirstOrDefault(x => x.Id == budgetLineId);

            if(budgetLine == null) {
                throw new ArgumentException("Budget line not found");
            }

            budgetLine.PaymentAmount = budgetLine.Amount;
        }
        await _unitOfWork.Repository<Domain.Entities.Budget>().UpdateEntry(entity);
        return _mapper.Map<Budget>(entity);
    }
}
