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

namespace RairBudgeting.UnitTests.Api.v1.Controllers;
[TestClass]
public class BudgetsControllerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<BudgetsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private static string validTestSubjectIdentifier = "12345";
    private static string invalidTestSubjectIdentifier = string.Empty;

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
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().List(validTestSubjectIdentifier)).ReturnsAsync(entities);
        SetupMapper<IEnumerable<RairBudgeting.Api.v1.DTOs.Budget>, IEnumerable<IBudget>>(dtos, entities);

        var results = _controller.List(string.Empty);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void List_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateListOfSize(5).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateListOfSize(5).Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().List(invalidTestSubjectIdentifier)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.List(string.Empty);

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void Create_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetAddCommand>.CreateNew().With(e => e.Id = Guid.NewGuid()).Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(returnDTO);

        var results = _controller.Create(requestDTO);

        AssertHttpStatusisValid<OkObjectResult>(results.Result);
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.Budget));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.Budget;
        Assert.IsNotNull(dto);

    }

    [TestMethod]
    public void Create_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetAddCommand>.CreateNew().With(e => e.Id = Guid.NewGuid()).Build();
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
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync(entities);

        var results = _controller.CreateBudgetLine(requestDTO.BudgetId, requestDTO);

        AssertHttpStatusisValid<OkResult>(results.Result);
    }

    [TestMethod]
    public void CreateLines_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.CreateBudgetLine(requestDTO.BudgetId, requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void GetById_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = Guid.NewGuid();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(id)).ReturnsAsync(entities);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(dtos, entities);

        var results = _controller.Get(id);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.Budget));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.Budget;
        Assert.IsNotNull(dto);

    }

    [TestMethod]
    public void GetById_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(id)).Returns(Task.FromResult((RairBudgeting.Api.Domain.Entities.Budget)null));
        var results = _controller.Get(id);

        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);    
    }

    [TestMethod]
    public void GetById_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var includedEnities = new List<string> { "Budget.BudgetLines" };
        var id = Guid.NewGuid();

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().Find(It.IsAny<BudgetWithLinesSpecification>())).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Get(id);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Delete_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetDeleteCommand>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(It.IsAny<BudgetDeleteCommand>(), default)).ReturnsAsync(true);

        var results = _controller.Delete(Guid.NewGuid());

        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
    }

    [TestMethod]
    public void Delete_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetDeleteCommand>.CreateNew().Build();
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Delete(requestDTO.Id);

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
        var id = Guid.NewGuid();
        SetupMapper<IBudget, RairBudgeting.Api.v1.DTOs.Commands.BudgetUpdateCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync(entities);
        var results = _controller.Update(id, requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
        var httpResult = results.Result as OkResult;

    }

    [TestMethod]
    public void Update_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetUpdateCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var id = Guid.NewGuid();
        SetupMapper<IBudget, BudgetUpdateCommand>(entities, requestDTO);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).Returns(Task.FromResult((RairBudgeting.Api.Domain.Entities.Budget)null));
        var results = _controller.Update(id, requestDTO);
        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);
    }

    [TestMethod]
    public void Update_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetUpdateCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var id = Guid.NewGuid();
        SetupMapper<IBudget, BudgetUpdateCommand>(entities, requestDTO);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Update(id, requestDTO);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Test_AddBudgetLineToBudgetCommand_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();

        SetupMapper<IBudget, AddBudgetLineToBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync(entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);

        var results = _controller.CreateBudgetLine(requestDTO.BudgetId, requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
        var httpResult = results.Result as OkResult;
    }

    [TestMethod]
    public void Test_AddBudgetLineToBudgetCommand_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var id = Guid.NewGuid();
        SetupMapper<IBudget, AddBudgetLineToBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync((RairBudgeting.Api.Domain.Entities.Budget)null);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);
        var results = _controller.CreateBudgetLine(id, requestDTO);
        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);
    }

    [TestMethod]
    public void Test_AddBudgetLineToBudgetCommand_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<IBudget, AddBudgetLineToBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));
        var results = _controller.CreateBudgetLine(requestDTO.BudgetId, requestDTO);
        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
        
    }

    [TestMethod]
    public void Test_DeleteBudgetLineFromBudgetCommand_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var budgetLineId = Guid.NewGuid();

        _mediatorMock.Setup(mock => mock.Send(It.IsAny<DeleteBudgetLineFromBudgetCommand>(), default)).ReturnsAsync(true);
        var results = _controller.DeleteBudgetLineFromBudget(entities.Id, budgetLineId);
        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
        var httpResult = results.Result as OkResult;
    }

    [TestMethod]
    public void Test_DeleteBudgetLineFromBudgetCommand_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<DeleteBudgetLineFromBudgetCommand>.CreateNew().Build();
        var budgetLineId = Guid.NewGuid();

        SetupMapper<IBudget, DeleteBudgetLineFromBudgetCommand>(entities, requestDTO);
        
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));
        var results = _controller.DeleteBudgetLineFromBudget(entities.Id, budgetLineId);
        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void Test_UpdateBudgetLineInBudgetCommand_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<UpdateBudgetLineInBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<IBudget, UpdateBudgetLineInBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync(entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);
        var results = _controller.UpdateBudgetLine(entities.Id, requestDTO);
        Assert.IsInstanceOfType(results.Result, typeof(OkResult));
        var httpResult = results.Result as OkResult;
    }

    [TestMethod]
    public void Test_UpdateBudgetLineInBudgetCommand_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<UpdateBudgetLineInBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        var id = Guid.NewGuid();
        SetupMapper<IBudget, UpdateBudgetLineInBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync((RairBudgeting.Api.Domain.Entities.Budget)null);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(true);
        var results = _controller.UpdateBudgetLine(id, requestDTO);
        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);
    }

    [TestMethod]
    public void Test_UpdateBudgetLineInBudgetCommand_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<UpdateBudgetLineInBudgetCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<IBudget, UpdateBudgetLineInBudgetCommand>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));
        var results = _controller.UpdateBudgetLine(entities.Id, requestDTO);
        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void Test_BudgetCloneCommand_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetCloneCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync(entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(returnDTO);
         var results = _controller.CloneBudget(entities.Id, requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;
    }

    [TestMethod]
    public void Test_BudgetCloneCommand_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetCloneCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().GetById(It.IsAny<Guid>())).ReturnsAsync((RairBudgeting.Api.Domain.Entities.Budget)null);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ReturnsAsync(returnDTO);
        var results = _controller.CloneBudget(entities.Id, requestDTO);
        var actionResult = results.Result as NotFoundResult;
        Assert.AreEqual(404, actionResult.StatusCode);
    }

    [TestMethod]
    public void Test_BudgetCloneCommand_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var requestDTO = Builder<BudgetCloneCommand>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(returnDTO, entities);
        _mediatorMock.Setup(mock => mock.Send(requestDTO, default)).ThrowsAsync(new ArgumentException("An error occured."));
        var results = _controller.CloneBudget(entities.Id, requestDTO);
        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }


}
