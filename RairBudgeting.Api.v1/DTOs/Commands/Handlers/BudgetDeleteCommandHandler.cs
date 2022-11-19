using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.v1.DTOs.Commands.Handlers;
public class BudgetDeleteCommandHandler : IRequestHandler<BudgetDeleteCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BudgetDeleteCommandHandler> _logger;

    public BudgetDeleteCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper, ILogger<BudgetDeleteCommandHandler> logger) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<bool> Handle(BudgetDeleteCommand request, CancellationToken cancellationToken) {
        try {
            var entityObject = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(request.BudgetId);
            entityObject.IsDeleted = true;

            await _unitOfWork.Repository<Domain.Entities.Budget>().Update(entityObject);
            return true;
        }
        catch(Exception ex) {
            _logger.Log(LogLevel.Error, ex, ex.Message);
            return false;
        }
    }
}
