using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Specifications;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class BudgetsController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BudgetsController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public BudgetsController(IUnitOfWork unitOfWork, ILogger<BudgetsController> logger, IMapper mapper, IMediator mediator) {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    } 

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> List(bool includeDeleted = false, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) {
        try {
            var entities = await _unitOfWork.Repository<Budget>().Get(x => x.IsDeleted == false || includeDeleted == true, orderBy: null, pageSize, pageIndex);

            return Ok(_mapper.Map<IEnumerable<DTOs.Budget>>(entities));
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpGet]
    [Route("matches")]

    public async Task<IActionResult> Find([FromQuery] int id) {
        try {
            var entity = await _unitOfWork.Repository<Budget>().Find();

            return Ok(_mapper.Map<IEnumerable<DTOs.Budget>>(entity));
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] IEnumerable<string> includedEntities) {
        try {
            var entities = await _unitOfWork.Repository<Budget>().Find(new BudgetWithLinesSpecification(id, includedEntities));
            var entity = entities.FirstOrDefault();
            return Ok(_mapper.Map<DTOs.Budget>(entity));
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpPost]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Create([FromBody] BudgetAddCommand newEntity) {
        try {
            var createdEntity = await _mediator.Send(newEntity);
            return Ok(createdEntity);
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpPost]
    [Route("BudgetLines")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Create([FromBody] AddBudgetLineToBudgetCommand newEntity) {
        var isCreated = await _mediator.Send(newEntity);

        return Ok();
    }

    [HttpPut]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Update([FromBody] BudgetUpdateCommand entity) {
        var isUpdated = await _mediator.Send(entity);

        return Ok(entity);
    }

    [HttpDelete]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Delete([FromQuery] int id) {
        try {
            var deleteCommand = new BudgetDeleteCommand(id);
            var isDeleted = await _mediator.Send(deleteCommand);
            return Ok();
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }
}
