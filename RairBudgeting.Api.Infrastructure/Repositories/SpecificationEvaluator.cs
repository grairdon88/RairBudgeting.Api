using Microsoft.EntityFrameworkCore;
using RairBudgeting.Api.Domain;
using RairBudgeting.Api.Domain.Interfaces.Specifications;
using RairBudgeting.Api.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RairBudgeting.Api.Infrastructure.Repositories;
public class SpecificationEvaluator<TEntity> where TEntity : Entity {
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification) {
        var query = inputQuery;

        //  Modify the IQueryable using the specification's criteria expression
        if(specification.Criteria != null) {
            query = query.Where(specification.Criteria);
        }

        //  Include all expression-based includes
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
    
        //  Include any string-based include statements
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));


        //  Apply ordering if expressions are set.
        if (specification.OrderBy != null) {
            query = query.OrderBy(specification.OrderBy);
        }
        else if(specification.OrderByDescending != null) {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if(specification.GroupBy != null) {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }

        if (specification.IsPagingEnabled) {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }
}
