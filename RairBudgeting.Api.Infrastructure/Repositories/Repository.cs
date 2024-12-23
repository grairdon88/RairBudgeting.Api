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

    public virtual async Task<IEnumerable<T>> Find(ISpecification<T>? specification = null) {
        if(specification == null) throw new ArgumentException("Specification cannot be null.", nameof(specification));

        return ApplySpecification(specification);
    }

    public virtual async Task DeleteById(int id) {
        var entity = await _dbSet.FindAsync(id);

        if(entity != null) {
            entity.IsDeleted = true;
            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageSize = 0, int pageIndex = 0,
        IEnumerable<string>? includedProperties = null) {
        if(pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be negative.");
        if(pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index cannot be negative.");

        IQueryable<T> query = _dbSet;

        if(filter != null) {
            query = query.Where(filter);
        }

        if(includedProperties != null) {
            foreach(var includedProperty in includedProperties) {
                if(string.IsNullOrWhiteSpace(includedProperty)) {
                    throw new ArgumentException("Included property cannot be null or whitespace.", nameof(includedProperties));
                }
                query = query.Include(includedProperty);
            }
        }

        if(orderBy != null) {
            query = orderBy(query);
        }

        if(pageSize > 0) {
            query = query.Skip(pageSize * pageIndex).Take(pageSize);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<T> GetById(int id) {
        var entity = await _dbSet.FindAsync(id);

        return entity;
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
