using FizzWare.NBuilder;
using RairBudgeting.Api.v1.DTOs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests.API.v1.Budgets;
public partial class BudgetsTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test4_Delete_200() {
        var dtoRequest = Builder<BudgetDeleteCommand>.CreateNew().Build();

        var response = await _client.DeleteAsync($"api/budgets?id={dtoRequest.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
