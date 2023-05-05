using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests;
public class IntegrationTestsBase {
    public static WebApplicationFactory<Program> _factory;
    public static HttpClient _client;
    [TestInitialize]
    public void TestInitialize() {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
            AllowAutoRedirect = false
        });
    }
}
