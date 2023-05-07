using FizzWare.NBuilder;
using RairBudgeting.Api.v1.DTOs;
using RairBudgeting.Api.v1.DTOs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests.API.v1.Budgets;
public partial class BudgetsTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test3_Update_200() {
        var createRequest = Builder<BudgetAddCommand>.CreateNew().With(e => e.BudgetTime = DateTime.Now).Build();

        var createResponse = await _client.PostAsJsonAsync("api/budgets", createRequest);
        var entity = await createResponse.Content.ReadFromJsonAsync<Budget>();

        var updateRequest = Builder<BudgetUpdateCommand>.CreateNew().With(e => e.Id = entity.Id).Build();
        var response = await _client.PutAsJsonAsync("api/budgets", updateRequest);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
