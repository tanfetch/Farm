using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Farm.AppCommon
{
    public static class IQqueryExtensions
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                     bool desc) where TEntity : class
        {
            string command = desc ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            if (property == null)
                return source;

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                   source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);

        }

        public static IQueryable<TEntity> Entities<TEntity>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> whereExp = null,
            string sort = "", string order = "",int curPage = 1, int pageSize = 0) where TEntity : class
        {
            var query = source;

            if (whereExp != null)
            {
                query = query.Where(whereExp);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                bool o = string.Equals(order, "asc");
                query = query.OrderBy(sort, o);
            }

            if (pageSize == 0)
                return query;

            return query.Skip((curPage - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<TEntity> Entities<TEntity>(this IQueryable<TEntity> source,
            out int total,
            Expression<Func<TEntity, bool>> whereExp = null,
            string sort = "", string order = "", int curPage = 1, int pageSize = 0) where TEntity : class
        {
            if (whereExp == null)
                total = source.Count();
            else
                total = source.Count(whereExp);

            total = source.Count(whereExp);
            return source.Entities(whereExp, sort, order, curPage, pageSize);

        }


    }
}
