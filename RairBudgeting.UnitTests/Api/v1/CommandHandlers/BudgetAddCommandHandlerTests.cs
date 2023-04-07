using FizzWare.NBuilder;
using Moq;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.DTOs.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api.v1.CommandHandlers;
[TestClass]

public class BudgetAddCommandHandlerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;

    private BudgetAddCommandHandler _handler;

    [TestInitialize]
    public void TestInit() {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);

        _handler = new BudgetAddCommandHandler(_unitOfWorkMock.Object, GetMapper().Object);
    }

    [TestMethod]
    public void Handle_Success() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.Budget>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Commands.BudgetAddCommand>.CreateNew().Build();
        var dto = Builder<RairBudgeting.Api.v1.DTOs.Budget>.CreateNew().Build();

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.Budget>().CreateEntry(entities)).ReturnsAsync(entities);
        SetupMapper<RairBudgeting.Api.v1.DTOs.Budget, IBudget>(dto, entities);
        SetupMapper<IBudget, RairBudgeting.Api.v1.DTOs.Commands.BudgetAddCommand>(entities, dtos);

        var results = _handler.Handle(dtos, CancellationToken.None);
        Assert.AreEqual(results.Status, TaskStatus.RanToCompletion);

        var returnedDTO = results.Result;
        Assert.IsNotNull(dto);
    }
}
