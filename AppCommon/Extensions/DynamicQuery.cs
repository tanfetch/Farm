using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;

namespace Farm.AppCommon
{

    public class DynamicQuery<T> where T : class
    {
        private List<FilterModel> filters = new List<FilterModel>();

        private Expression<Func<T, bool>> GetWhereExpress()
        {
            Expression<Func<T, bool>> expression = PredicateExtensions.True<T>();

            if (filters == null)
                return expression;

            foreach (var filter in filters)
            {
                PropertyInfo Tp = typeof(T).GetProperty(filter.propertyName);
                if (Tp == null)
                    continue;

                var keyworld = Convert.ChangeType(filter.keyWord, Tp.PropertyType);
                var right = Expression.Constant(keyworld);

                var itemParameter = Expression.Parameter(typeof(T));
                var left = Expression.Property(itemParameter, filter.propertyName);

                if (filter.filterFlag == FilterFlagEnum.Equals)
                {
                    var ep = Expression.Equal(left, right);
                    var whereExpression = Expression.Lambda<Func<T, bool>>(ep, new[] { itemParameter });
                    expression = expression.And(whereExpression);
                }
                else if (filter.filterFlag == FilterFlagEnum.min)
                {
                    var ep = Expression.GreaterThanOrEqual(left, right);
                    var whereExpression = Expression.Lambda<Func<T, bool>>(ep, new[] { itemParameter });
                    expression = expression.And(whereExpression);
                }
                else if (filter.filterFlag == FilterFlagEnum.max)
                {
                    var ep = Expression.LessThanOrEqual(left, right);
                    var whereExpression = Expression.Lambda<Func<T, bool>>(ep, new[] { itemParameter });
                    expression = expression.And(whereExpression);
                }
                else if (filter.filterFlag == FilterFlagEnum.Containsed)
                {
                    MethodInfo method = left.Type.GetMethod("Contains");
                    if (method != null)
                    {
                        var whereExpression = Expression.Lambda<Func<T, bool>>(
                            Expression.Call(left, method, new[] { right }), new[] { itemParameter });
                        expression = expression.And(whereExpression);
                    }
                }
                else if (filter.filterFlag == FilterFlagEnum.Contains)
                {
                    MethodInfo method = right.Type.GetMethod("Contains");
                    if (method != null)
                    {
                        var whereExpression = Expression.Lambda<Func<T, bool>>(
                            Expression.Call(right, method, new[] { left }), new[] { itemParameter });
                        expression = expression.And(whereExpression);
                    }
                }

            }

            return expression;
        }

        public Expression<Func<T, bool>> whereExp
        {
            get { return GetWhereExpress(); }
        }

        public int UpdateModel(NameValueCollection nvc)
        {
            foreach (string n in nvc.AllKeys)
            {
                var v = nvc[n];
                if (string.IsNullOrEmpty(n) || string.IsNullOrEmpty(v))
                    continue;

                int f = 0;
                var x = n.LastIndexOf('_');
                if (x <= 0)
                    continue;

                var p = n.Substring(0, x);
                if (!int.TryParse(n.Substring(x + 1), out f))
                    continue;

                FilterModel filter = new FilterModel(v, p, f);

                filters.Add(filter);
            }

            return 0;
        }
    }

    internal class FilterModel
    {
        public FilterModel(string k, string p, int f)
        {
            keyWord = k;
            propertyName = p;
            filterFlag = (FilterFlagEnum)f;
        }
        public string keyWord { get; set; }
        public string propertyName { get; set; }
        public FilterFlagEnum filterFlag { get; set; }
    }

    internal enum FilterFlagEnum
    {
        Equals = 0,
        min = 1,
        max = 2,
        Contains = 3,
        Containsed = 4
    }
}
