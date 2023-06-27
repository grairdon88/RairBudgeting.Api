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
        ItemResponse<T> responseEntity = await _dbContainer.ReadItemAsync<T>(id.ToString(), partitionKey: new PartitionKey(id.ToString()));

        return responseEntity.Resource;
    }

    public virtual async Task<IEnumerable<T>> List(string subjectIdentifier) {
        return _dbContainer.GetItemLinqQueryable<T>(true).Where(x => x.UserId == subjectIdentifier).AsEnumerable();
    }

    public virtual async Task Update(T entity) {
        await _dbContainer.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey));
    }

    public virtual async Task<T> UpdateEntry(T entity) {
        await _dbContainer.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey));

        return entity;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) {
        throw new NotImplementedException();
    }
}
