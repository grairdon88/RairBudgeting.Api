using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Domain.Specifications;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Controllers;
using RairBudgeting.Api.v1.DTOs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api.v1.Controllers;
[TestClass]
public class BudgetsControllerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<BudgetsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;

    private BudgetsController _controller;

    [TestInitialize]
    public void TestInit() {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<BudgetsController>>(MockBehavior.Strict);
        _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);

        _controller = new BudgetsController(_unitOfWorkMock.Object, _loggerMock.Object, GetMapper().Object, _mediatorMock.Object);
    }

    [TestMethod]
    public void List_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateListOfSize(5).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateListOfSize(5).Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().List()).ReturnsAsync(entities);
        SetupMapper<IEnumerable<RairBudgeting.Api.v1.DTOs.Budget>, IEnumerable<IBudget>>(dtos, entities);

        var results = _controller.List();

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void List_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateListOfSize(5).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateListOfSize(5).Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().List()).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.List();

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void Create_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetAddCommand>.CreateNew().With(e => e.Id = 0).Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(returnDTO);

        var results = _controller.Create(requestDTO);

        AssertHttpStatusisValid<OkObjectResult>(results.Result);
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.Budget));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.Budget;
        Assert.IsNotNull(dto);
        Assert.AreEqual(entities.Id, dto.Id);

    }

    [TestMethod]
    public void Create_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetAddCommand>.CreateNew().With(e => e.Id = 0).Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Create(requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void CreateLines_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);

        var results = _controller.Create(requestDTO.BudgetId, requestDTO);

        AssertHttpStatusisValid<OkResult>(results.Result);
    }

    [TestMethod]
    public void CreateLines_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Create(requestDTO.BudgetId, requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void GetById_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateListOfSize(1).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = 1;
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().Find(It.IsAny<BudgetWithLinesSpecification>())).ReturnsAsync(entities);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(dtos, entities[0]);

        var results = _controller.Get(id, includedEnities);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.Budget));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.Budget;
        Assert.IsNotNull(dto);
        Assert.AreEqual(entities[0].Id, dto.Id);

    }

    [TestMethod]
    public void GetById_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = 1;

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().Find(It.IsAny<BudgetWithLinesSpecification>())).Returns(Task.FromResult((IEnumerable<RairBudgeting.Api.Domain.Entities.Budget>)null));
        var results = _controller.Get(id, includedEnities);

        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);    
    }

    [TestMethod]
    public void GetById_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = 1;

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().Find(It.IsAny<BudgetWithLinesSpecification>())).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Get(id, includedEnities);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Delete_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetDeleteCommand>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(It.IsAny<BudgetDeleteCommand>(), default)).ReturnsAsync(true);

        var results = _controller.Delete(1);

        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
    }

    [TestMethod]
    public void Delete_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetDeleteCommand>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Delete(requestDTO.BudgetId);

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void Update_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetUpdateCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        //_unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(requestDTO.Id)).ReturnsAsync(entities);
        //_unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().Update(entities)).Returns(Task.CompletedTask);
        //_unitOfWorkMock.Setup(mock => mock.CompleteAsync()).ReturnsAsync(1);

        SetupMapper<IBudget, RairBudgeting.Api.v1.DTOs.Commands.BudgetUpdateCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);

        var results = _controller.Update(requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
        var httpResult = results.Result as OkResult;

        //Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.BudgetCategory));
        //var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.BudgetCategory;
        //Assert.IsNotNull(dto);
        //Assert.AreEqual(entities.Id, dto.Id);

    }

    [TestMethod]
    public void Update_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetUpdateCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<IBudget, BudgetUpdateCommand>(entities, requestDTO);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Update(requestDTO);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Update_404() {
        //var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        //var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        //_unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(requestDTO.Id)).Returns(Task.FromResult((RairBudgeting.Api.Domain.Entities.BudgetCategory)null));

        //SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);

        //var results = _controller.Update(requestDTO);

        //Assert.IsInstanceOfType(results.Result, typeof(NotFoundResult));

    }
}
