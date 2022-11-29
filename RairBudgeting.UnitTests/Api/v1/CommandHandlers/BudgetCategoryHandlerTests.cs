using FizzWare.NBuilder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Controllers;
using RairBudgeting.Api.v1.DTOs.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.UnitTests.Api.v1.CommandHandlers;
[TestClass]
internal class BudgetCategoryHandlerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<BudgetCategoriesController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
     
    private BudgetCategoryAddCommandHandler _controller;

    [TestInitialize]
    public void TestInit() {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<BudgetCategoriesController>>(MockBehavior.Strict);
        _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);

        _controller = new BudgetCategoryAddCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object, GetMapper().Object);
    }

    //[TestMethod]
    //public void List_200() {
    //    var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateListOfSize(5).Build();
    //    var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateListOfSize(5).Build();
    //    _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().List()).ReturnsAsync(entities);
    //    SetupMapper<IEnumerable<RairBudgeting.Api.v1.DTOs.BudgetCategory>, IEnumerable<IBudgetCategory>>(dtos, entities);
    //    BudgetCategoryAddCommandHandler handler = new BudgetCategoryAddCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object, GetMapper().Object);

    //    var results = handler.Handle(new);

    //    Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));

    //}
}
