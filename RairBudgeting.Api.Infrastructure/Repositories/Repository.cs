using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Domain.Interfaces.Specifications;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;

namespace RairBudgeting.Api.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : Entity {
    private readonly Container _dbContainer;
    public Repository(Container dbContainer) {
        _dbContainer = dbContainer;
    }
    
    public virtual async Task<T> Create(T entity) {

        return entity;
    }

    public virtual async Task<T> CreateEntry(T entity) {
        ItemResponse<T> createResponse = await _dbContainer.CreateItemAsync(entity, new PartitionKey(entity.PartitionKey));
        

        return createResponse.Resource;
    }

    public virtual async Task<IEnumerable<T>> Find(ISpecification<T> specification = null) {
        return ApplySpecification(specification);
    }

    public virtual async Task DeleteById(Guid id) {
        throw new NotImplementedException();
    }
     
    public virtual async Task<T> GetById(Guid id) {
        //var responseEntity = await _dbContainer.ReadItemAsync<dynamic>(id.ToString(), partitionKey: new PartitionKey(id.ToString()));
        ItemResponse<T> responseEntity = await _dbContainer.ReadItemAsync<T>(id.ToString(), partitionKey: new PartitionKey(id.ToString()));

        return responseEntity.Resource;
    }

    public virtual async Task<IEnumerable<T>> List(string subjectIdentifier) {
        var entityItems = new List<T>();
        var query = new QueryDefinition($"SELECT DISTINCT * FROM c WHERE c.userId = \"{subjectIdentifier}\"");

        var resultSetIterator = _dbContainer.GetItemQueryIterator<T>(query);

        while (resultSetIterator.HasMoreResults) {
            FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
            
            foreach(var item in response) {
                entityItems.Add(item);
            }
        }

        return entityItems;
    }

    public virtual async Task Update(T entity) {
        await _dbContainer.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey));
    }

    public virtual async Task<T> UpdateEntry(T entity) {
        await _dbContainer.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey));

        return entity;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) {
        //return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        throw new NotImplementedException();
    }
}
