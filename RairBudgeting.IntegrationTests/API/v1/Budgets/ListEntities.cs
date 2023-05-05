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
    public async Task Test2_List_200() {
        var response = await _client.GetAsync("api/budgets/list");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var entities = await response.Content.ReadFromJsonAsync<IEnumerable<Budget>>();
        Assert.IsInstanceOfType(entities, typeof(IEnumerable<Budget>));
    }
}