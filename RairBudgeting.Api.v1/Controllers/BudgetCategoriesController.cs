using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.DTOs.Commands;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
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
    public async Task<IActionResult> List() {
        try {
            var entities = await _unitOfWork.Repository<BudgetCategory>().List();
            var filteredEntities = entities.Where(e => e.IsDeleted == false);

            return Ok(_mapper.Map<IEnumerable<v1.DTOs.BudgetCategory>>(filteredEntities));
        }
        catch (Exception ex) {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Detail = ex.Message
            });
        }
     }

    //[HttpGet]
    //[Route("matches")]
    //public async Task<IActionResult> Find([FromBody] int id) {
    //    try {
    //        var entity = await _unitOfWork.Repository<BudgetCategory>().Find();

    //        return Ok(entity);
    //    }
    //    catch (Exception ex) {
    //        return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
    //            Status = StatusCodes.Status500InternalServerError,
    //            Title = "An unexpected error occured.",
    //            Detail = ex.Message
    //        });
    //    }
    //}

    [HttpGet]
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
    public async Task<IActionResult> Create([FromBody] DTOs.Commands.BudgetCategoryAddCommand newEntity) {
        try {
            //if(ModelState.IsValid == false) {
            //    return new BadRequestObjectResult(new ValidationProblemDetails());
            //}
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

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DTOs.Commands.BudgetCategoryUpdateCommand entity) {

        try {
            var existingEntity = await _unitOfWork.Repository<BudgetCategory>().GetById(entity.Id);

            if (existingEntity == null) {
                return new NotFoundResult();
            }

            var createdEntity = await _mediator.Send(existingEntity);

            await _unitOfWork.Repository<BudgetCategory>().Update(_mapper.Map<Domain.Entities.BudgetCategory>(existingEntity));
            await _unitOfWork.CompleteAsync();
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

    [HttpDelete]
    [SwaggerResponse(200, "Successful operation", Type = typeof(DTOs.Budget))]
    public async Task<IActionResult> Delete([FromQuery] int id) {
        try {
            var deleteCommand = new BudgetCategoryDeleteCommand(id);
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
