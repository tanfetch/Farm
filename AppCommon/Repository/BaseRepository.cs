using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Farm.AppCommon
{
    public class BaseRepository<TBase> 
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

        virtual protected IQueryable<T> EntitiesAll<T>() where T : class,TBase
        {
            DataContext dc = new DataContext(connection);
            dc.ObjectTrackingEnabled = false;
            return dc.GetTable<T>();
        }

        virtual public T GetEntitie<T>(Expression<Func<T, bool>> whereExp) where T : class,TBase
        {
            var query = EntitiesAll<T>().AsQueryable();
            Expression<Func<T, bool>> True = ( f => true );
            return query.FirstOrDefault(True.And(whereExp));
        }

        virtual public int GetCount<T>(Expression<Func<T, bool>> whereExp) where T : class,TBase
        {
            var query = EntitiesAll<T>().AsQueryable();
            Expression<Func<T, bool>> True = (f => true);
            return query.Count(True.And(whereExp).Compile());
        }

        virtual public IQueryable<T> GetEntities<T>(
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0) where T : class,TBase
        {
            var query = this.EntitiesAll<T>();
            return query.Entities(whereExp, sort, order,curPage, pageSize);
        }

        virtual public IQueryable<T> GetEntities<T>(out int total,
            Expression<Func<T, bool>> whereExp = null,
            string sort = null, string order = null, int curPage = 1, int pageSize = 0) where T : class,TBase
        {
            var query = this.EntitiesAll<T>();

            return query.Entities(out total,whereExp, sort, order, curPage, pageSize);
        }

        virtual public string Update<T>(T bllTable) where T : class,IBaseTable
        {
            using (DataContext dc = new DataContext(connection))
            {
                bool isInsert = dc.GetTable<T>().Count(p=>p.ID == bllTable.ID)==0;
                if (isInsert)
                {
                    dc.GetTable<T>().InsertOnSubmit(bllTable);
                }
                else
                {
                    dc.GetTable<T>().Attach(bllTable, false);
                    dc.Refresh(RefreshMode.KeepCurrentValues, bllTable);
                }

                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    return e.Message;
                }

                return string.Empty;
            }
        }

        virtual public string Inset<T>(T bllTable) where T : class,TBase
        {
            using (DataContext dc = new DataContext(connection))
            { 
                try
                {
                    dc.GetTable<T>().InsertOnSubmit(bllTable);
                    dc.SubmitChanges();
                }
                catch(Exception e)
                {
                    return e.Message;
                }
            }

            return string.Empty;
        }

        virtual public string UpdateProperty<T>(Func<T, bool> exp, string property, object value) where T : class,TBase
        {
            using (DataContext dc = new DataContext(connection))
            {
                var model = dc.GetTable<T>().Where(exp);
                if (model.Count() == 0)
                    return "更新的数据不存在";

                foreach (var m in model)
                {
                    var f = m.GetType().GetProperty(property);
                    if (f != null)
                    {
                        f.SetValue(m, Convert.ChangeType(value, f.PropertyType), null);
                    }
                }

                try
                {
                    dc.SubmitChanges();
                }
                catch(Exception e)
                {
                    return e.Message;
                }

                return string.Empty;
            }
        }

        virtual public string Delete<T>(Func<T, bool> exp) where T : class,TBase
        {
            using (DataContext dc = new DataContext(connection))
            {
                
                var bllTable = dc.GetTable<T>().Where(exp);
                if (bllTable.Count() == 0)
                    return string.Empty;
                
                try
                {
                    dc.GetTable<T>().DeleteAllOnSubmit(bllTable);
                    dc.SubmitChanges();
                }
                catch(Exception e)
                {
                    return e.Message;
                }
            }

            return string.Empty;
        }

    }
}
