using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RairBudgeting.Api.v1.DTOs;

namespace RairBudgeting.IntegrationTests.API.v1.BudgetCategories;
public partial class BudgetCategoriesTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test3_Update_200() {
        var dtoRequest = Builder<BudgetCategory>.CreateNew().With(e => e.Name = DateTime.Now.ToString()).Build();

        var response = await _client.PutAsJsonAsync("api/budgetcategories", dtoRequest);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
