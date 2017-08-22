using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Farm.AppCommon
{
/*
    public class BaseRepository<T>: IBaseRepository<T>  where T : class,IBaseTable
    {
        public BaseRepository()
        {
            this.connection = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public BaseRepository(string fileOrServerOrConnection)
        {
            this.connection = fileOrServerOrConnection;
        }

        virtual protected string connection { get; set; }

        virtual protected IQueryable<T> EntitiesAll()
        {
            DataContext dc = new DataContext(connection);
            dc.ObjectTrackingEnabled = false;
            return dc.GetTable<T>();
        }

        virtual public T GetEntitie(Expression<Func<T, bool>> whereExp)
        {
            var query = EntitiesAll().AsQueryable();
            return query.FirstOrDefault(whereExp);
        }

        virtual public IQueryable<T> GetEntities(
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0)
        {
            var query = this.EntitiesAll();
            return query.GetEntities(whereExp, sort, order,curPage, pageSize);
        }

        virtual public IQueryable<T> GetEntities(out int total,
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0)
        {
            var query = this.EntitiesAll();
            total = query.Count(whereExp);
            return query.GetEntities(whereExp, sort, order, curPage, pageSize);
        }


        public int Inset(T bllTable)
        {
            if (bllTable == null)
                return 1;

            using (DataContext dc = new DataContext(connection))
            {
                try
                {
                    dc.GetTable<T>().InsertOnSubmit(bllTable);
                    dc.SubmitChanges();
                }
                catch
                {
                    return 2;
                }
            }

            return 0;
        }

        public int Update(T bllTable)
        {
            if (bllTable == null)
                return 1;

            using (DataContext dc = new DataContext(connection))
            {
                dc.GetTable<T>().Attach(bllTable, false);
                dc.Refresh(RefreshMode.KeepCurrentValues, bllTable);

                try
                {
                    dc.SubmitChanges();
                }
                catch
                {
                    return 2;
                }


            }

            return 0;
        }

        public int UpdateField(Func<T, bool> exp, string field, object value)
        {
            using (DataContext dc = new DataContext(connection))
            {
                var model = dc.GetTable<T>().Where(exp);
                if (model.Count() == 0)
                    return 1;

                foreach (var m in model)
                {
                    var f = m.GetType().GetProperty(field);
                    if (f != null)
                    {
                        f.SetValue(m, Convert.ChangeType(value, f.PropertyType), null);
                    }
                }

                try
                {
                    dc.SubmitChanges();
                }
                catch
                {
                    return 2;
                }
                return 0;
            }
        }

        public int Delete(Func<T, bool> exp)
        {
            using (DataContext dc = new DataContext(connection))
            {
                var bllTable = dc.GetTable<T>().Where(exp);
                if (bllTable.Count() == 0)
                    return 1;

                try
                {
                    dc.GetTable<T>().DeleteAllOnSubmit(bllTable);
                    dc.SubmitChanges();
                }
                catch
                {
                    return 2;
                }
            }
            return 0;
        }

    }
 */
}
