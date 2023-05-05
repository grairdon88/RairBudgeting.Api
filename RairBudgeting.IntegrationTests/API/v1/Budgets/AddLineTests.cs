using FizzWare.NBuilder;
using RairBudgeting.Api.v1.DTOs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests.API.v1.Budgets;
public partial class BudgetsTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test5_AddLine_200() {
        var dtoRequest = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();

        var response = await _client.PostAsJsonAsync($"api/budgets/{dtoRequest.BudgetId}/budgetlines", dtoRequest);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task Test6_AddBudgetLine_To_Budget_200() {
        var dtoRequest = Builder<AddBudgetLineToBudgetCommand>.CreateNew().Build();
        var response = await _client.PostAsJsonAsync($"api/budgets/{dtoRequest.BudgetId}/budgetlines", dtoRequest);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
