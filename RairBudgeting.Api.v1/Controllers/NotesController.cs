using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;

namespace RairBudgeting.Api.v1.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class NotesController : Controller {
    private IUnitOfWork _unitOfWork;
    private readonly ILogger<NotesController> _logger;

    public NotesController(IUnitOfWork unitOfWork, ILogger<NotesController> logger) {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    //[HttpGet]
    //[Route("list")]
    //public async Task<IActionResult> List() {
    //    try {
    //        var entities = await _unitOfWork.Repository<Note>().List();

    //        return Ok(entities);
    //    }
    //    catch (Exception ex) {
    //        return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails {
    //            Status = StatusCodes.Status500InternalServerError,
    //            Title = "An unexpected error occured.",
    //            Detail = ex.Message
    //        });
    //    }
    //}

    //[HttpGet]
    //public async Task<IActionResult> Get([FromQuery] int id) {
    //    try {
    //        var entity = await _unitOfWork.Repository<Note>().GetById(id);

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

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] Note newEntity) {

    //    var createdEntity = await _unitOfWork.Repository<Note>().Create(newEntity);

    //    return Ok(createdEntity);
    //}

    //[HttpPut]
    //public async Task<IActionResult> Update([FromBody] Note newEntity) {

    //    await _unitOfWork.Repository<Note>().Update(newEntity);

    //    return Ok(newEntity);
    //}
}
