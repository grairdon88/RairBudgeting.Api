using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using RairBudgeting.Api.v1.DTOs;
using RairBudgeting.Api.v1.DTOs.Commands;

namespace RairBudgeting.IntegrationTests.API.v1.BudgetCategories;
public partial class BudgetCategoriesTests : IntegrationTestsBase {
    [TestMethod]
    public async Task Test1_Create_200() {
        var dtoRequest = Builder<BudgetCategoryAddCommand>.CreateNew().Build();

        var response = await _client.PostAsJsonAsync("api/budgetcategories", dtoRequest);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var entity = await response.Content.ReadFromJsonAsync<BudgetCategory>();
        Assert.IsInstanceOfType(entity, typeof(BudgetCategory));
        Assert.AreNotEqual(string.Empty, entity.Id);
    }
}
