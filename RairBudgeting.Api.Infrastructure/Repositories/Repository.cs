using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Interfaces.Specifications;
using RairBudgeting.Api.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : Entity {
    private readonly BudgetContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(BudgetContext context) {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dbSet = _context.Set<T>();
    }
    
    public virtual async Task<T> Create(T entity) {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<T> CreateEntry(T entity) {
        _context.Entry(entity).State = EntityState.Added;
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<IEnumerable<T>> Find(ISpecification<T> specification = null) {
        return ApplySpecification(specification);
    }

    public virtual async Task DeleteById(int id) {
        
    }

    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includedProperties = "") {
        IQueryable<T> query = _dbSet;

        if(filter != null) {
            query = query.Where(filter);
        }

        foreach (var includedProperty in includedProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
            query = query.Include(includedProperty);
        };

        if(orderBy != null) {
            return orderBy(query).ToList();
        }
        else {
            return query.ToList();
        }
    }

    public virtual async Task<T> GetById(int id) {
        var entity = await _dbSet.FindAsync(id);

        return entity;
    }

    public virtual async Task<IEnumerable<T>> List() {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task Update(T entity) {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<T> UpdateEntry(T entity) {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return entity;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}
