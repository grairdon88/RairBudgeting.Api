using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Controllers;
using RairBudgeting.Api.v1.DTOs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api.v1;
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

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        //Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.Budget));
        //var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.Budget;
        //Assert.IsNotNull(dto);
        //Assert.AreEqual(entities.Id, dto.Id);

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
    public void Delete_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetDeleteCommand>.CreateNew().Build();
        //_unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().Create(entities)).ReturnsAsync(entities);
        //_unitOfWorkMock.Setup(mock => mock.CompleteAsync()).ReturnsAsync(1);
        _mediatorMock.Setup(mock => mock.Send(It.IsAny<BudgetDeleteCommand>(), default)).ReturnsAsync(true);
        //SetupMapper<IBudget, BudgetAddCommand>(entities, requestDTO);
        //SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);

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
}
