using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RairBudgeting.Api.Domain.Interfaces.Entities;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using RairBudgeting.Api.v1.Controllers;
using RairBudgeting.Api.v1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests.API.v1.BudgetCategories;

public partial class BudgetCategoriesTests: IntegrationTestsBase {
    [TestMethod]
    public async Task Test2_List_200() {
        var response = await _client.GetAsync("api/budgetcategories/list");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var entities = await response.Content.ReadFromJsonAsync<IEnumerable<BudgetCategory>>();
        Assert.IsInstanceOfType(entities, typeof(IEnumerable<BudgetCategory>));
    }
}
