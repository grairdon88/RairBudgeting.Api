using Microsoft.EntityFrameworkCore;
using Moq;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Entities;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Infrastructure;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using static Dapper.SqlMapper;

namespace RairBudgeting.UnitTests.Infrastructure;

[TestClass]
public class UnitOfWorkTests {
    private IUnitOfWork _unitOfWorkMock;
    private Mock<BudgetContext> _contextMock;

    [TestInitialize]
    public void TestInit() {
        var dbContextOptions = new DbContextOptionsBuilder<BudgetContext>()
            .UseInMemoryDatabase(databaseName: "Budget")
            .Options;
        _contextMock = new Mock<BudgetContext>(MockBehavior.Strict, dbContextOptions);
        _unitOfWorkMock = new UnitOfWork(_contextMock.Object);

    }

    [TestMethod]
    public void CompleteAsync_Success() {
        _contextMock.Setup(mock => mock.SaveChangesAsync(default)).ReturnsAsync(1);
        var results = _unitOfWorkMock.CompleteAsync();

        Assert.AreEqual(results.Result, 1);
    }

    [TestMethod]
    public void DisposeAsync_DisposesSuccessfully() {
        _contextMock.Setup(mock => mock.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var results = _unitOfWorkMock.DisposeAsync();

        Assert.IsTrue(results.IsCompletedSuccessfully);
    }

    //[TestMethod]
    //public void Repository_Runs_Successfully() {
    //    Mock<IRepository<IEntity>> repo = new Mock<IRepository<IEntity>>();

    //    //  This line needs mocked.
    //    //var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
    //    //_contextMock.Setup(mock => mock.Make).Returns(ValueTask.CompletedTask);

    //    //_unitOfWorkMock.Setup(mock => mock.Repository<IEntity>()).Returns(repo.Object);
    //    var entity = _unitOfWorkMock.Repository<BudgetCategory>();

    //    Assert.IsInstanceOfType(repo.Object, typeof(Repository<>));
    //}

    //[TestMethod]
    //public void Repository_Runs() {
    //    Mock<IRepository<IEntity>> repo = new Mock<IRepository<IEntity>>();

    //    //_unitOfWorkMock.Setup(mock => mock.Repository<IEntity>()).Returns(repo.Object);
    //    var entity = _unitOfWorkMock.Repository<IEntity>();

    //    Assert.IsInstanceOfType(repo.Object, typeof(IRepository<IEntity>));
    //}
}