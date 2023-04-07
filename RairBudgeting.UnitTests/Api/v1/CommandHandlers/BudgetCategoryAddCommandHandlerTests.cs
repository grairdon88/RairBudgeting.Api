using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Controllers;
using RairBudgeting.Api.v1.DTOs;
using RairBudgeting.Api.v1.DTOs.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api.v1.CommandHandlers;
[TestClass]
public class BudgetCategoryAddCommandHandlerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;
     
    private BudgetCategoryAddCommandHandler _handler;

    [TestInitialize]
    public void TestInit() {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);

        _handler = new BudgetCategoryAddCommandHandler(_unitOfWorkMock.Object, GetMapper().Object);
    }

    [TestMethod]
    public void Handle_Success() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.Commands.BudgetCategoryAddCommand>.CreateNew().Build();
        var dto = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();

        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().CreateEntry(entities)).ReturnsAsync(entities);
        SetupMapper<RairBudgeting.Api.v1.DTOs.BudgetCategory, IBudgetCategory>(dto, entities);
        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.Commands.BudgetCategoryAddCommand>(entities, dtos);

        var results = _handler.Handle(dtos, CancellationToken.None);
        Assert.AreEqual(results.Status, TaskStatus.RanToCompletion);

        var returnedDTO = results.Result;
        Assert.IsNotNull(dto);
    }
}
