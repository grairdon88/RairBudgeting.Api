using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.v1.Commands.BudgetLines;
using RairBudgeting.Api.v1.Commands.Budgets;
using Swashbuckle.AspNetCore.Annotations;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class BudgetsController : ControllerBase {
    private readonly ILogger<BudgetsController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public BudgetsController(ILogger<BudgetsController> logger, IMapper mapper, IMediator mediator) {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("list")]
    [SwaggerOperation(
        Summary = "Lists out all Budgets.",
        Description = "Lists out all Budgets by the provided filter criteria.",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.", typeof(IEnumerable<DTOs.BudgetCategory>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List(bool includeDeleted = false, [FromQuery] int pageSize = 0, [FromQuery] int pageIndex = 0, [FromQuery] IEnumerable<string> includedProperties = null) {
        try {
            includedProperties ??= new List<string>();

            var budgetListCommand = new BudgetListCommand {
                IncludeDeleted = includeDeleted,
                PageSize = pageSize,
                PageIndex = pageIndex,
                IncludedProperties = includedProperties
            };

            var entities = await _mediator.Send(budgetListCommand);

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

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Gets an existing Budget by the provided ID.",
        Description = "Retrieves a Budget by ID.",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.", typeof(DTOs.Budget))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] IEnumerable<string> includedProperties) {
        try {
            var budgetGetCommand = new BudgetGetCommand {
                Id = id,
                IncludedEntities = includedProperties
            };

            var entity = await _mediator.Send(budgetGetCommand);

            if (entity == null)
                return NotFound();

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
        Summary = "Creates a Budget.",
        Description = "Creates a Budget with the provided data.",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status201Created, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
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
    [Route("{budgetId}/BudgetLines")]
    [SwaggerOperation(
        Summary = "Creates a Budget Line",
        Description = "Creates a Budget Line and associates it with a Budget entity. ",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromRoute] int budgetId, [FromBody] AddBudgetLineToBudgetCommand newEntity) {
        var getBudgetCommand = new BudgetGetCommand {
            Id = budgetId
        };
        var budget = await _mediator.Send(getBudgetCommand);

        if(budget == null)
            return NotFound();

        var isCreated = await _mediator.Send(newEntity);

        return Ok();
    }

    [HttpPut]
    [SwaggerOperation(
        Summary = "Marks provided Budget Line(s) as paid.",
        Description = "Takes in a list of Budget Line IDs and marks them as paid for a specified Budget Entity.",
        Tags = new[] { "Budgets" }
    )]
    [Route("{budgetId}/BudgetLines/paid")]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkBudgetLinesPaid([FromRoute] int budgetId, [FromBody] MarkBudgetLineAsPaidCommand requestBody) {
        //  Get the budget
        var getBudgetCommand = new BudgetGetCommand {
            Id = budgetId
        };
        var budget = await _mediator.Send(getBudgetCommand);

        if(budget == null)
            return NotFound();

        //  Set the budget ID for processing the payments
        requestBody.BudgetId = budgetId;
        var isProcessed = await _mediator.Send(requestBody);

        if(isProcessed == false)
            return NotFound();

        return Ok();
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Updates an existing Budget",
        Description = "Updates an existing Budget.",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The provided Budget ID could not be found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BudgetUpdateCommand entity) {
        var budgetGetCommand = new BudgetGetCommand {
            Id = id
        };

        var isUpdated = await _mediator.Send(entity);

        return Ok();
    }

    [HttpDelete]
    [SwaggerOperation(
        Summary = "Deletes Budget",
        Description = "Deletes a list of Budget by provided IDs.",
        Tags = new[] { "Budgets" }
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Operation completed successfully.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
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
