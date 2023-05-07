using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.v1.DTOs.Commands;
using RairBudgeting.Api.v1.DTOs;

namespace RairBudgeting.IntegrationTests.API.v1.Budgets;
public partial class BudgetsTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test1_Create_200() {
        var dtoRequest = Builder<BudgetAddCommand>.CreateNew().Build();

        var response = await _client.PostAsJsonAsync("api/budgets", dtoRequest);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var entity = await response.Content.ReadFromJsonAsync<Budget>();
        Assert.IsInstanceOfType(entity, typeof(Budget));
        Assert.AreNotEqual(string.Empty, entity.Id);
    }
}
