using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{

    public Expression<Func<T, bool>>? Criteria { get; private set; }

    public List<Expression<Func<T, object>>> Includes { get; } = [];

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; } = false;

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (Criteria != null)
        {
            query = query.Where(Criteria);
        }

        return query;
    }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
