using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Repositories;
public class UnitOfWork(BudgetContext context) : IUnitOfWork {
    private readonly BudgetContext _context = context;
    private Hashtable? _repositories;

    public async Task<int> CompleteAsync() {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync() { 
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity {
        if (_repositories == null) {
            _repositories = new Hashtable();
        }

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type)) {
            var repositoryType = typeof(Repository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            if(repositoryInstance != null) _repositories.Add(type, repositoryInstance);
        }

        return _repositories[type] as IRepository<TEntity> ?? throw new InvalidOperationException("The provided entity is not a valid entity for data retrieval.");
    }
}
