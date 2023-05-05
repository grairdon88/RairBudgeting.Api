using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using RairBudgeting.Api.v1.DTOs;

namespace RairBudgeting.IntegrationTests.API.v1.BudgetCategories;
public partial class BudgetCategoriesTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test4_Delete_200() {
        var dtoRequest = Builder<BudgetCategory>.CreateNew().Build();

        var response = await _client.DeleteAsync($"api/budgetcategories?id={dtoRequest.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
