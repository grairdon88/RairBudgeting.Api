using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;

namespace RairBudgeting.Api.v1.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BudgetCategoriesController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BudgetCategoriesController> _logger;

    public BudgetCategoriesController(IUnitOfWork unitOfWork, ILogger<BudgetCategoriesController> logger, IMapper mapper) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("list")]
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
    public async Task<IActionResult> Create([FromBody] DTOs.BudgetCategory newEntity) {
        try {
            //if(ModelState.IsValid == false) {
            //    return new BadRequestObjectResult(new ValidationProblemDetails());
            //}
            var createdEntity = await _unitOfWork.Repository<BudgetCategory>().Create(_mapper.Map<Domain.Entities.BudgetCategory>(newEntity));
            await _unitOfWork.CompleteAsync();
            return Ok(_mapper.Map<DTOs.BudgetCategory>(createdEntity));
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
    public async Task<IActionResult> Update([FromBody] DTOs.BudgetCategory newEntity) {

        try {
            var entity = await _unitOfWork.Repository<BudgetCategory>().GetById(newEntity.Id);

            if (entity == null) {
                return new NotFoundResult();
            }

            await _unitOfWork.Repository<BudgetCategory>().Update(_mapper.Map<Domain.Entities.BudgetCategory>(newEntity));
            await _unitOfWork.CompleteAsync();
            return Ok(newEntity);
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
