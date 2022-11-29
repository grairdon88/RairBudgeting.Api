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
public class BudgetCategoryDeleteCommandHandler : IRequestHandler<BudgetCategoryDeleteCommand, bool> {
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BudgetDeleteCommandHandler> _logger;

    public BudgetCategoryDeleteCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IMapper mapper, ILogger<BudgetDeleteCommandHandler> logger) {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> Handle(BudgetCategoryDeleteCommand request, CancellationToken cancellationToken) {
        try {
            var entityObject = await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().GetById(request.BudgetCategoryId);
            entityObject.IsDeleted = true;

            await _unitOfWork.Repository<Domain.Entities.BudgetCategory>().Update(entityObject);
            return true;
        }
        catch (Exception ex) {
            _logger.Log(LogLevel.Error, ex, ex.Message);
            return false;
        }
    }
}
