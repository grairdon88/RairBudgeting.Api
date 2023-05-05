using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.IntegrationTests;
public class RairBudgetingAPIFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            //var dbContextDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbContextOptions<ApplicationDbContext>));

            //services.Remove(dbContextDescriptor);

            //var dbConnectionDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbConnection));

            //services.Remove(dbConnectionDescriptor);

            //// Create open SqliteConnection so EF won't automatically close it.
            //services.AddSingleton<DbConnection>(container => {
            //    var connection = new SqliteConnection("DataSource=:memory:");
            //    connection.Open();

            //    return connection;
            //});
            //services.AddSingleton<CosmosClient>(serviceProvider => {
            //    return new CosmosClient(services.Configuration.GetConnectionString("DefaultConnection"));
            //});

        });

        builder.UseEnvironment("Development");
    }
}
