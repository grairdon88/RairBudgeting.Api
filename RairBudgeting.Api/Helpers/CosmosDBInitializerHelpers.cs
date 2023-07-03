using Azure.Identity;
using Microsoft.Azure.Cosmos;

namespace RairBudgeting.Api.Helpers;

public class CosmosDBInitializerHelpers {
    public static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfiguration configuration) {
        var containersToInitialize = new List<(string, string)> { ("RairBudgeting", "Budget"), ("RairBudgeting", "BudgetCategory") };
        var cosmosClient = await CosmosClient.CreateAndInitializeAsync(configuration.GetConnectionString("DefaultConnection"), containersToInitialize);
        await cosmosClient.CreateDatabaseIfNotExistsAsync("RairBudgeting");
        return cosmosClient;
    }
}
