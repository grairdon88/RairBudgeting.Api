using RairBudgeting.Api.Domain.Interfaces.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
public interface IRepository<T> {
    Task<IEnumerable<T>> List();
    Task<IEnumerable<T>> Find(ISpecification<T> specificaton = null);
    Task<T> GetById(Guid id);
    Task<T> Create(T entity);
    Task<T> CreateEntry(T entity);
    Task Update(T entity);
    Task<T> UpdateEntry(T entity);
    Task DeleteById(Guid id);
}
