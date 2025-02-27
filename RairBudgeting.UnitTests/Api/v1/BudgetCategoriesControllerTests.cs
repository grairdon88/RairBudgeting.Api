﻿using Moq;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using RairBudgeting.Api.v1.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using FizzWare.NBuilder;


namespace RairBudgeting.UnitTests.Api.v1;
[TestClass]
public class BudgetCategoriesControllerTests : UnitTestBase {
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<BudgetCategoriesController>> _loggerMock;

    private BudgetCategoriesController _controller;

    [TestInitialize]
    public void TestInit() {
        _unitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<BudgetCategoriesController>>(MockBehavior.Strict);

        _controller = new BudgetCategoriesController(_unitOfWorkMock.Object, _loggerMock.Object, GetMapper().Object);
    }

    [TestMethod]
    public void List_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateListOfSize(5).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateListOfSize(5).Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().List()).ReturnsAsync(entities);
        SetupMapper<IEnumerable<RairBudgeting.Api.v1.DTOs.BudgetCategory>, IEnumerable<IBudgetCategory>>(dtos, entities);

        var results = _controller.List();

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));

    }

    [TestMethod]
    public void List_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateListOfSize(5).Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateListOfSize(5).Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().List()).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.List();

        Assert.IsInstanceOfType(results.Result, typeof(ObjectResult));

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);
    }

    [TestMethod]
    public void GetById_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        var id = 1;
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(id)).ReturnsAsync(entities);
        SetupMapper<RairBudgeting.Api.v1.DTOs.BudgetCategory, IBudgetCategory>(dtos, entities);

        var results = _controller.Get(id);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.BudgetCategory));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.BudgetCategory;
        Assert.IsNotNull(dto);
        Assert.AreEqual(entities.Id, dto.Id);

    }

    [TestMethod]
    public void GetById_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        var id = 1;
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(id)).Returns(Task.FromResult((RairBudgeting.Api.Domain.Entities.BudgetCategory)null));

        var results = _controller.Get(id);

        var actionResult = results.Result as NotFoundResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(404, actionResult.StatusCode);

    }

    [TestMethod]
    public void GetById_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var dtos = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        var id = 1;
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(id)).ThrowsAsync(new ArgumentException("An error occured."));

        var results = _controller.Get(id);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Create_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().With(e => e.Id = 0).Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().Create(entities)).ReturnsAsync(entities);
        _unitOfWorkMock.Setup(mock => mock.CompleteAsync()).ReturnsAsync(1);
        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.BudgetCategory, IBudgetCategory>(returnDTO, entities);

        var results = _controller.Create(requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.BudgetCategory));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.BudgetCategory;
        Assert.IsNotNull(dto);
        Assert.AreEqual(entities.Id, dto.Id);

    }

    [TestMethod]
    public void Create_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().With(e => e.Id = 0).Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().Create(entities)).ThrowsAsync(new ArgumentException("An error occured."));
        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);

        var results = _controller.Create(requestDTO);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

    [TestMethod]
    public void Update_200() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(requestDTO.Id)).ReturnsAsync(entities);
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().Update(entities)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(mock => mock.CompleteAsync()).ReturnsAsync(1);
        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);
        SetupMapper<RairBudgeting.Api.v1.DTOs.BudgetCategory, IBudgetCategory>(returnDTO, entities);

        var results = _controller.Update(requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(OkObjectResult));
        var httpResult = results.Result as OkObjectResult;

        Assert.IsInstanceOfType(httpResult.Value, typeof(RairBudgeting.Api.v1.DTOs.BudgetCategory));
        var dto = httpResult.Value as RairBudgeting.Api.v1.DTOs.BudgetCategory;
        Assert.IsNotNull(dto);
        Assert.AreEqual(entities.Id, dto.Id);

    }

    [TestMethod]
    public void Update_404() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().GetById(requestDTO.Id)).Returns(Task.FromResult((RairBudgeting.Api.Domain.Entities.BudgetCategory)null));

        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);

        var results = _controller.Update(requestDTO);

        Assert.IsInstanceOfType(results.Result, typeof(NotFoundResult));

    }

    [TestMethod]
    public void Update_500() {
        var entities = Builder<RairBudgeting.Api.Domain.Entities.BudgetCategory>.CreateNew().Build();
        var requestDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        var returnDTO = Builder<RairBudgeting.Api.v1.DTOs.BudgetCategory>.CreateNew().Build();
        _unitOfWorkMock.Setup(mock => mock.Repository<RairBudgeting.Api.Domain.Entities.BudgetCategory>().Update(entities)).ThrowsAsync(new ArgumentException("An error occured."));
        SetupMapper<IBudgetCategory, RairBudgeting.Api.v1.DTOs.BudgetCategory>(entities, requestDTO);

        var results = _controller.Update(requestDTO);

        var actionResult = results.Result as ObjectResult;
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(500, actionResult.StatusCode);

    }

}
