using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Interfaces;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
public interface IUnitOfWork : IAsyncDisposable {
    IRepository<TEntity> Repository<TEntity>() where TEntity : IEntity;
    //Task<int> CompleteAsync();
}
