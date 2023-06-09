using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.DTOs;

namespace RairBudgeting.Api.v1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : Controller {
    private IUnitOfWork _unitOfWork;
    private readonly ILogger<NotesController> _logger;
    private readonly IMediator _mediator;

    public NotesController(IUnitOfWork unitOfWork, ILogger<NotesController> logger, IMediator mediator) {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> List(bool includeDeleted = false) {
        try {
            var subjectIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var entities = await _unitOfWork.Repository<Domain.Entities.Note>().List(subjectIdentifier);
            var filteredEntities = entities.Where(e => e.IsDeleted == false || includeDeleted == true);
            return Ok(filteredEntities);
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
    public async Task<IActionResult> GetByID(Guid id) {
        try {
            var entity = await _unitOfWork.Repository<Domain.Entities.Note>().GetById(id);

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
}
