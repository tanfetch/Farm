using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Farm.AppCommon
{
    public interface IBaseRepository<TBase>
    {

        T GetEntitie<T>(Expression<Func<T, bool>> whereExp) where T : class,TBase;

        IQueryable<T> GetEntities<T>(out int total,
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0) where T : class,TBase;

        IQueryable<T> GetEntities<T>(
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0) where T : class,TBase;
    }
}
