using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.DTOs.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
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
    public async Task<IActionResult> List(string userId, bool includeDeleted = false) {
        try {
            var entities = await _unitOfWork.Repository<Domain.Entities.Budget>().List(userId);
            var filteredEntities = entities.Where(e => e.IsDeleted == false || includeDeleted == true);

            return Ok(_mapper.Map<IEnumerable<DTOs.Budget>>(filteredEntities));
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
    public async Task<IActionResult> Get([FromRoute] Guid id) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if(entity == null)
                return NotFound();
            
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

    [HttpPost("{id}/clone")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> CloneBudget([FromRoute] Guid id, BudgetCloneCommand budgetCloneCommand) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if (entity == null)
                return NotFound();

            var createdEntity = await _mediator.Send(budgetCloneCommand);
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
    [Route("{id}/BudgetLines")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> CreateBudgetLine([FromQuery] Guid id, [FromBody] AddBudgetLineToBudgetCommand newEntity) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if (entity == null)
                return NotFound();

            var isCreated = await _mediator.Send(newEntity);
            return Ok();

        }
        catch(Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    // Controller endpont to update a budget line.
    [HttpPut]
    [Route("{id}/BudgetLines")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> UpdateBudgetLine([FromRoute] Guid id, [FromBody] UpdateBudgetLineInBudgetCommand updatedEntity) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if (entity == null)
                return NotFound();

            var isUpdated = await _mediator.Send(updatedEntity);
            return Ok();
        }
        catch(Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }
    // Controller endpoint to delete a budget line.

    [HttpDelete]
    [Route("{id}/BudgetLines")]
    public async Task<IActionResult> DeleteBudgetLineFromBudget([FromRoute] Guid id, [FromQuery] Guid budgetLineId) {
        try {
            var deleteCommand = new DeleteBudgetLineFromBudgetCommand(id, budgetLineId);
            var isDeleted = await _mediator.Send(deleteCommand);
            return Ok();
        }
        catch(Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] BudgetUpdateCommand budgetUpdateCommand) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if (entity == null)
                return NotFound();

            var isUpdated = await _mediator.Send(budgetUpdateCommand);

            return Ok();
        }
        catch(Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpDelete]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Delete([FromQuery] Guid id) {
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

    [HttpPut("{id}/budgetlines/paid")]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    // controller action to mark multiple budget lines as paid.
    public async Task<IActionResult> MarkBudgetLinesAsPaid([FromRoute] Guid id, [FromBody] MarkBudgetLinesAsPaidCommand markBudgetLinesAsPaidCommand) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Budget>().GetById(id);

            if (entity == null)
                return NotFound();

            var isUpdated = await _mediator.Send(markBudgetLinesAsPaidCommand);
            return Ok();
        }
        catch(Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }
}
