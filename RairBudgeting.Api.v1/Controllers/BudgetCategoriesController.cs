using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands;
using RairBudgeting.Api.v1.Commands.BudgetCategories;
using RairBudgeting.Api.v1.CommandHandlers.BudgetCategories;
using Swashbuckle.AspNetCore.Annotations;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class BudgetCategoriesController : ControllerBase {
    private readonly IMapper _mapper;
    private readonly ILogger<BudgetCategoriesController> _logger;
    private readonly IMediator _mediator;

    public BudgetCategoriesController(ILogger<BudgetCategoriesController> logger, IMapper mapper, IMediator mediator) {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("list")]
    [SwaggerOperation(
        Summary = "Lists out all Budget Categories.",
        Description = "Lists out all Budget Categories by the provided filter criteria.",
        Tags = new[] { "Budget Categories" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.", typeof(IEnumerable<DTOs.BudgetCategory>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List([FromQuery] bool includeDeleted = false, [FromQuery] int pageSize = 0, [FromQuery] int pageIndex = 0) {
        try {
            var listCommand = new BudgetCategoryListCommand {
                IncludeDeleted = includeDeleted,
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            var entities = await _mediator.Send(listCommand);

            return Ok(entities);
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
    [SwaggerOperation(
        Summary = "Gets an existing Budget Category by the provided ID.",
        Description = "Retrieves a Budget Category by ID.",
        Tags = new[] { "Budget Categories" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.", typeof(DTOs.BudgetCategory))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget Category ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery]int id) {
        try {
            var getCommand = new BudgetCategoryGetCommand {
                Id = id
            };

            var entity = await _mediator.Send(getCommand);

            if(entity == null) {
                return NotFound();
            }

            return Ok(entity);
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
    [SwaggerOperation(
        Summary = "Creates a Budget Category.",
        Description = "Creates a Budget Category with the provided data.",
        Tags = new[] { "Budget Categories" }
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] BudgetCategoryAddCommand newEntity) {
        try {
            var createdEntity = await _mediator.Send(newEntity);
            return Created(string.Empty, createdEntity);
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary ="Updates an existing Budget Category",
        Description = "Updates an existing Budget Category.",
        Tags = new[] {"Budget Categories"}
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget Category ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] [SwaggerParameter("An updated budget category.")] BudgetCategoryUpdateCommand updatedEntity) {

        try {
            var getCommand = new BudgetCategoryGetCommand {
                Id = id
            };

            var entity = await _mediator.Send(getCommand);
            
            if (entity == null) {
                return new NotFoundResult();
            }

            var isUpdated = await _mediator.Send(updatedEntity);
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
    [HttpDelete]
    [SwaggerOperation(
        Summary = "Deletes Budget Categories",
        Description = "Deletes a list of Budget Categoreis by provided IDs.",
        Tags = new[] { "Budget Categories" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromQuery] List<int> id) {
        try {
            var deleteCommand = new BudgetCategoryDeleteCommand { Id = id };
            var operationCompleted = await _mediator.Send(deleteCommand);
            
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
