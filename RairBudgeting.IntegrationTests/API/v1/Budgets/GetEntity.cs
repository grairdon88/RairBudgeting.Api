using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.v1.DTOs;

namespace RairBudgeting.IntegrationTests.API.v1.Budgets;
public partial class BudgetsTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test4_GetByID_200() {
        var response = await _client.GetAsync($"api/budgets?id=332339af-7abb-42e0-b8e6-0ce03d63107f");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var entities = await response.Content.ReadFromJsonAsync<Budget>();
        Assert.IsInstanceOfType(entities, typeof(Budget));
    }

    public async Task Test_GetByID_200() {
        var response = await _client.GetAsync($"api/budgets?id=332339af-7abb-42e0-b8e6-0ce03d63107f");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        var entities = await response.Content.ReadFromJsonAsync<Budget>();
        Assert.IsInstanceOfType(entities, typeof(Budget));
    }
}
