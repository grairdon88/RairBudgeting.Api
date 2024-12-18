using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Budget Categories")]
[ApiController]
public class BudgetCategoriesController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BudgetCategoriesController> _logger;
    private readonly IMediator _mediator;

    public BudgetCategoriesController(IUnitOfWork unitOfWork, ILogger<BudgetCategoriesController> logger, IMapper mapper, IMediator mediator) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List([FromQuery] bool includeDeleted = false, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0) {
        try {
            var entities = await _unitOfWork.Repository<BudgetCategory>().Get(x => x.IsDeleted == false || includeDeleted == true, orderBy: null, pageSize, pageIndex);

            return Ok(_mapper.Map<IEnumerable<v1.DTOs.BudgetCategory>>(entities));
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery]int id) {
        try {
            var entity = await _unitOfWork.Repository<BudgetCategory>().GetById(id);

            if(entity == null) {
                return NotFound();
            }

            return Ok(_mapper.Map<v1.DTOs.BudgetCategory>(entity));
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpPut]
    [SwaggerOperation(
        Summary ="Updates an existing Budget Category",
        Description = "Updates an existing Budget Category."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] [SwaggerParameter("An updated budget category.")] DTOs.BudgetCategory updatedEntity) {

        try {
            var entity = await _unitOfWork.Repository<BudgetCategory>().GetById(updatedEntity.Id);

            if (entity == null) {
                return new NotFoundResult();
            }

            await _unitOfWork.Repository<BudgetCategory>().Update(_mapper.Map<Domain.Entities.BudgetCategory>(updatedEntity));
            await _unitOfWork.CompleteAsync();
            return Ok(updatedEntity);
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
    public async Task<IActionResult> Delete([FromQuery] List<int> id) {
        try {

            foreach (var entityID in id) {
                await _unitOfWork.Repository<BudgetCategory>().DeleteById(entityID);
            }
            await _unitOfWork.CompleteAsync();
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
