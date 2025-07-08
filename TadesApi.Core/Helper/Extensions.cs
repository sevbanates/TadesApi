using TadesApi.Core.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace TadesApi.CoreHelper
{
    public static class Extensions
    {
        public static TSource FirstOrNew<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                var t = (TSource)Activator.CreateInstance(typeof(TSource));
                return t;
            }

            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
            }

            return default(TSource);
        }

        public static IQueryable<T> ExecutePredicate<T>(this IQueryable<T> source, Expression<Func<T, bool>> generalPredicate)
        {
            var query = source;
            if (generalPredicate.Body.ToString().NE("False"))
                query = query.Where(generalPredicate);

            return query;
        }

        public static IQueryable<T> SetPaging<T>(this IQueryable<T> source, PagedAndSortedModel<T> qParams)
        {
            var query = source;

            query = qParams.SortDirection.ToLower().Equals("asc")
                ? query.OrderBy(qParams.SortBy)
                : query.OrderByDescending(qParams.SortBy);

            query = query.Skip((qParams.PageNumber - 1) * qParams.PageSize).Take(qParams.PageSize);
            return query;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query,
            string sortField,
            string sortDirection)
        {
            return sortDirection.ToLower().EQ(SortDirection.Asc)
                ? query.OrderBy(s => s.GetType().GetProperty(sortField))
                : query.OrderByDescending(s => s.GetType().GetProperty(sortField));
        }

        public static class PropertyHelper<T>
        {
            //*** PropertyInfo prop = PropertyHelper<Foo>.GetProperty(x => x.Bar);
            public static PropertyInfo GetProperty<TValue>(
                Expression<Func<T, TValue>> selector)
            {
                Expression body = selector;
                if (body is LambdaExpression)
                {
                    body = ((LambdaExpression)body).Body;
                }

                switch (body.NodeType)
                {
                    case ExpressionType.MemberAccess:
                        return (PropertyInfo)((MemberExpression)body).Member;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public static List<T> ToListReadUncommitted<T>(this IQueryable<T> query)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
            var toReturn = query.ToList();
            scope.Complete();
            return toReturn;
        }

        public static int CountReadUncommitted<T>(this IQueryable<T> query)
        {
            using (var scope = new TransactionScope(
                       TransactionScopeOption.Required,
                       new TransactionOptions()
                       {
                           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                       }))
            {
                int toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }


        private static object GetReflectedPropertyValue(this object subject, string field)
        {
            return subject.GetType().GetProperty(field).GetValue(subject, null);
        }

        //***************************** List<T>  ***********************************
        public static T XAddItem<T>(this List<T> list) where T : new()
        {
            var obj = new T();
            list.Add(obj);
            return obj;
        }

        public static List<T> XAdd<T>(this List<T> list, T obj)
        {
            list.Add(obj);
            return list;
        }

        //***************************** IEnumerable<T>  ***********************************
        public static IEnumerable<T> XGetFirst<T>(this IEnumerable<T> thislist, int index = 1)
        {
            var i = 0;
            foreach (var item in thislist)
            {
                i++;
                if (i > index) break;
                yield return item;
            }
        }

        //***************************** IEnumerable<string> ***********************************
        public static string XJoin(this IEnumerable<string> tObj, string seperator)
        {
            return string.Join(seperator, tObj);
        }

        public static string XFirst(this IEnumerable<string> tObj)
        {
            var rval = tObj.FirstOrDefault();
            if (rval == null)
                return "";

            return rval;
        }

        public static string XJoin(this IEnumerable<long> tObj, string seperator)
        {
            return string.Join(seperator, tObj.ToString());
        }

        public static IEnumerable<string> XTrim(this IEnumerable<string> tObj, bool removeEmtyOnes = true)
        {
            foreach (var item in tObj)
            {
                if (removeEmtyOnes && item.Trim() == string.Empty)
                    continue;
                yield return item.Trim();
            }
        }

        public static string XLast(this IEnumerable<string> tObj)
        {
            var rval = tObj.LastOrDefault();
            if (rval == null)
                return string.Empty;

            return rval;
        }

        public static IEnumerable<string> XNotIn(this IEnumerable<string> tObj, IEnumerable<string> list2,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var list = new List<string>();
            foreach (var item in tObj)
            {
                if (!list2.Any(x => x.Equals(item, comparison)))
                    list.Add(item);
            }

            return list;
        }

        public static bool XEmpty(this IEnumerable<string> tObj)
        {
            return !tObj.Any(x => !string.IsNullOrEmpty(x));
        }

        public static string XGetAt(this IEnumerable<string> tObj, int index)
        {
            if (index == 0)
                throw new ArgumentException("XElement-0 cannot be used. Use:1,2,3,-1,-2,-3...");

            //*** index values are: 1,2,3 / -1,-2
            var parts = tObj.ToArray();
            if (index > 0)
            {
                if (index > parts.Length)
                    return string.Empty;

                return parts[index - 1];
            }
            else
            {
                var rindex = -index;
                if (rindex > parts.Length)
                    return string.Empty;

                return parts[parts.Length - rindex];
            }
        }

        public static bool XContains(this IEnumerable<string> tval, string value)
        {
            if (value == null)
                return false;

            foreach (var item in tval.Where(x => x != null))
            {
                if (item.EQ(value)) return true;
            }

            return false;
        }

        public static bool IsNotInitial(this IEnumerable<string> tObj)
        {
            return !tObj.IsInitial();
        }

        public static bool IsNotInitial<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return false;

            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return !(collection.Count < 1);
            }

            return enumerable.Any();
        }

        public static bool IsInitial<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            var collection = enumerable as ICollection<T>;
            if (collection != null)
            {
                return (collection.Count == 0);
            }

            return !enumerable.Any();
        }

        public static bool IsInitial(this IEnumerable<string> tObj)
        {
            //*** Checks any non-empty string.
            if (tObj == null)
                return true;

            foreach (var item in tObj)
            {
                if (!string.IsNullOrEmpty(item))
                    return false;
            }

            return true;
        }

        public static bool IsNegative(this IEnumerable<int> tObj)
        {
            if (tObj == null)
                return true;

            foreach (var item in tObj)
            {
                if (item < 0)
                    return true;
            }

            return false;
        }

        public static bool IsNegative(this IEnumerable<decimal> tObj)
        {
            if (tObj == null)
                return true;

            foreach (var item in tObj)
            {
                if (item < 0)
                    return true;
            }

            return false;
        }

        public static bool IsNegative(this int? tObj)
        {
            if (tObj == null)
                return true;

            if (tObj < 0)
                return true;
            return false;
        }

        public static bool IsNegative(this decimal? tObj)
        {
            if (tObj == null)
                return true;

            if (tObj < 0)
                return true;
            return false;
        }

        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }


        //***************************** String ***********************************
        public static string XGetCode(this string tval)
        {
            return CommonFunctions.GetCode(tval);
        }

        public static string XGetCodes(this string tval)
        {
            return CommonFunctions.GetCodes(tval);
        }

        public static string XStrip(this string tval, Regex regex)
        {
            tval = regex.Replace(tval, "");
            return tval;
        }

        public static string[] XSplit(this string tval, string firstSep, params string[] seps)
        {
            foreach (var sep in seps)
                tval = tval.Replace(sep, firstSep);

            return CommonFunctions.Split(tval, firstSep);
        }

        public static List<string> XSplit(this string tval, int len)
        {
            var list = new List<string>();

            if (string.IsNullOrEmpty(tval))
                return list;

            if (len < 1)
                return list;

            for (var i = 0; i < tval.Length; i += len)
            {
                if (len + i > tval.Length)
                    len = tval.Length - i;

                list.Add(tval.Substring(i, len));
            }

            return list;
        }

        public static string XLeft(this string tval, int len)
        {
            if (string.IsNullOrEmpty(tval))
                return string.Empty;

            if (tval.Length <= len)
                return tval;

            return tval.Substring(0, len);
        }

        public static string XRight(this string tval, int len)
        {
            if (string.IsNullOrEmpty(tval))
                return string.Empty;

            if (tval.Length < len)
                return tval;

            return tval.Substring(tval.Length - len);
        }

        public static bool XBegWith(this string tval, string str2)
        {
            if (string.IsNullOrEmpty(tval))
                return false;

            if (string.IsNullOrEmpty(str2))
                return false;

            if (tval.Length < str2.Length)
                return false;

            if (tval.XLeft(str2.Length).EQ(str2))
                return true;

            return false;
        }

        public static int ToIntWithoutChar(this string value)
        {
            return CommonFunctions.ToIntWithoutChar(value);
        }

        public static decimal ToNum(this string value)
        {
            return CommonFunctions.ToNum(value);
        }

        public static long ToLong(this string value)
        {
            return CommonFunctions.ToLong(value);
        }

        public static int ToInt(this string value)
        {
            return CommonFunctions.ToInt(value);
        }

        //public static long ToInt64(this string value)
        //{
        //    return Functions.ToInt64(value);
        //}
        public static DateTime ToDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return CommonFunctions.DefDate;

            DateTime date;
            if (DateTime.TryParse(value, out date))
                return date;
            else
                return CommonFunctions.DefDate;
        }

        public static string XBase64Encode(this string tval)
        {
            return CommonFunctions.Base64Encode(tval);
        }

        public static string XBase64Decode(this string tval)
        {
            return CommonFunctions.Base64Decode(tval);
        }

        public static bool LIKE(this string thisObj, string value)
        {
            if (thisObj == null || value == null)
                return false;

            return thisObj.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool SLIKE(this string thisObj, string value)
        {
            if (thisObj == null || value == null)
                return false;

            return thisObj.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ELIKE(this string thisObj, string value)
        {
            if (thisObj == null || value == null)
                return false;

            return thisObj.EndsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EQ(this string thisObj, string value)
        {
            if (thisObj == null || value == null)
                return false;

            return thisObj.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EQ(this Guid thisObj, Guid value)
        {
            var thisObj1 = thisObj.ToString();
            var value1 = value.ToString();

            return thisObj1.Equals(value1, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EQ(this Guid thisObj, Guid? value)
        {
            if (value == null)
                return false;

            var thisObj1 = thisObj.ToString();
            var value1 = value.ToString();

            return thisObj1.Equals(value1, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool EQ(this Guid? thisObj, Guid? value)
        {
            if (thisObj == null || value == null)
                return false;

            var thisObj1 = thisObj.ToString();
            var value1 = value.ToString();

            return thisObj1.Equals(value1, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool NE(this Guid thisObj, Guid value)
        {
            return !EQ(thisObj.ToString(), value.ToString());
        }


        public static bool NE(this string thisObj, string value)
        {
            return !EQ(thisObj, value);
        }

        public static bool GT(this string thisObj, string value)
        {
            return string.Compare(thisObj, value, true) > 0;
        }

        public static bool LT(this string thisObj, string value)
        {
            return string.Compare(thisObj, value, true) < 0;
        }

        public static bool GE(this string thisObj, string value)
        {
            return string.Compare(thisObj, value, true) >= 0;
        }

        public static bool LE(this string thisObj, string value)
        {
            return string.Compare(thisObj, value, true) <= 0;
        }

        public static bool IN(this string thisObj, List<string> values)
        {
            return thisObj.IN_(values);
        }

        public static bool IN(this string thisObj, params string[] values)
        {
            return thisObj.IN_(values);
        }

        private static bool IN_(this string thisObj, IEnumerable<string> values)
        {
            if (thisObj == null)
                return false;

            if (values == null)
                return false;

            foreach (var item in values)
            {
                if (thisObj.Equals(item, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        public static bool NotIN(this string thisObj, params string[] values)
        {
            return !IN(thisObj, values);
        }

        public static bool IsInitial(this string thisObj)
        {
            if (thisObj == null)
                return true;

            return thisObj == string.Empty;
        }

        public static bool IsInitial(this byte thisObj)
        {
            return thisObj == 0;
        }

        public static bool IsInitial(this byte? thisObj)
        {
            return thisObj == null;
        }

        public static bool IsNotInitial(this string thisObj)
        {
            return !(IsInitial(thisObj));
        }

        public static bool IsNumeric(this string thisObj)
        {
            return CommonFunctions.IsNumeric(thisObj);
        }

        //***************************** DateTime ***********************************
        public static bool EQ(this DateTime thisObj, DateTime value, string format = null)
        {
            return thisObj.ToString(format).EQ(value.ToString(format));
        }

        public static bool BetWeen(this DateTime thisObj, DateTime value1, DateTime value2)
        {
            if (thisObj <= value2 && thisObj >= value1)
                return true;

            return false;
        }

        public static bool BetWeen(this DateTime? thisObj, DateTime? value1, DateTime? value2)
        {
            if (thisObj <= value2 && thisObj >= value1)
                return true;

            return false;
        }

        public static bool NE(this DateTime thisObj, DateTime value, string format = null)
        {
            return !EQ(thisObj, value, format);
        }

        public static bool EQ(this DateTime? thisObj, DateTime value, string format = null)
        {
            if (thisObj == null)
                return false;
            var myObj = thisObj ?? CommonFunctions.DefDate;

            return myObj.ToString(format).EQ(value.ToString(format));
        }

        public static bool NE(this DateTime? thisObj, DateTime value, string format = null)
        {
            return !EQ(thisObj, value, format);
        }

        public static bool IsInitial(this DateTime thisObj)
        {
            return thisObj == CommonFunctions.DefDate;
        }

        public static bool IsInitial(this DateTime? thisObj)
        {
            if (thisObj == null)
                return true;

            return thisObj == CommonFunctions.DefDate;
        }

        public static bool IsNotInitial(this DateTime thisObj)
        {
            return !(IsInitial(thisObj));
        }

        public static bool IsNotInitial(this DateTime? thisObj)
        {
            return !(IsInitial(thisObj));
        }


        public static string ToLocalStr(this DateTime thisObj, string timeformat = "")
        {
            return CommonFunctions.CnvtDateToText(thisObj, timeformat);
        }

        public static bool IsNull(this DateTime? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this DateTime thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this decimal thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this decimal? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this string thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this int? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNull(this int thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this DateTime? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this DateTime thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this decimal thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this decimal? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this string thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this int? thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        public static bool IsNotNull(this int thisObj)
        {
            throw new NotImplementedException("Only for db use!");
        }

        //***************************** Double ***********************************
        public static double XFirst(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.First();
        }

        public static double XMax(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Max();
        }

        public static double XMin(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Min();
        }

        public static double XSum(this IEnumerable<double> tObj)
        {
            if (tObj.Count() == 0)
                return 0.0;

            return tObj.Sum();
        }

        public static bool IsInitial(this decimal thisObj)
        {
            return thisObj.EQ(CommonFunctions.DefNum);
        }

        public static bool IsNotInitial(this decimal thisObj)
        {
            return !thisObj.IsInitial();
        }

        public static bool EQ(this double thisObj, double p)
        {
            return (decimal)thisObj == (decimal)p;
        }

        public static bool EQ(this int thisObj, int p)
        {
            var resp = (int)thisObj == (int)p;
            return resp;
        }

        public static bool EQ(this int? thisObj, int? p)
        {
            return (int)thisObj == (int)p;
        }


        public static bool NE(this double thisObj, double p)
        {
            return !thisObj.EQ(p);
        }

        public static bool GT(this double thisObj, double p)
        {
            return thisObj > p;
        }

        public static bool LT(this double thisObj, double p)
        {
            return thisObj < p;
        }

        public static bool GE(this double thisObj, double p)
        {
            if (thisObj.EQ(p))
                return true;

            return thisObj.GT(p);
        }

        public static bool LE(this double thisObj, double p)
        {
            if (thisObj.EQ(p))
                return true;

            return thisObj.LT(p);
        }

        public static bool IN(this double thisObj, params double[] values)
        {
            return values.Any(value => thisObj.EQ(value));
        }

        public static bool NotIN(this double thisObj, params double[] values)
        {
            return !IN(thisObj, values);
        }

        //------- Double-II
        public static string ToLocalStr(this decimal thisObj, string format = "")
        {
            return CommonFunctions.CnvtNumberToText(thisObj, format);
        }

        public static decimal ToDecimal(this decimal thisObj, string format)
        {
            return (decimal)thisObj.ToString(format).ToNum();
        }

        //***************************** Int32 ***********************************
        public static int XFirst(this IEnumerable<int> tObj)
        {
            if (tObj.Count() == 0)
                return 0;

            return tObj.First();
        }

        public static int XMax(this IEnumerable<int> tObj)
        {
            if (tObj.Count() == 0)
                return 0;

            return tObj.Max();
        }

        public static int XMin(this IEnumerable<int> tObj)
        {
            if (tObj.Count() == 0)
                return 0;

            return tObj.Min();
        }

        public static int XSum(this IEnumerable<int> tObj)
        {
            if (tObj.Count() == 0)
                return 0;

            return tObj.Sum();
        }

        public static bool IsInitial(this int? thisObj)
        {
            return thisObj == 0 || thisObj == null;
        }

        public static bool IsInitial(this long? thisObj)
        {
            return thisObj == 0 || thisObj == null;
        }

        public static bool IsNotInitial(this long? thisObj)
        {
            return !(thisObj == 0 || thisObj == null);
        }

        public static bool IsNotInitial(this int? thisObj)
        {
            return !thisObj.IsInitial();
        }

        public static bool IsNotInitial(this decimal? thisObj)
        {
            return !thisObj.IsInitial();
        }

        public static bool IsInitial(this decimal? thisObj)
        {
            if (thisObj == null)
                return true;

            return thisObj == CommonFunctions.DefNum;
        }

        public static bool IsInitial(this int thisObj)
        {
            return thisObj == 0;
        }

        public static bool IsInitial(this long thisObj)
        {
            return thisObj == 0;
        }

        public static bool IsNotInitial(this int thisObj)
        {
            return !thisObj.IsInitial();
        }

        public static bool IsNotInitial(this long thisObj)
        {
            return !thisObj.IsInitial();
        }

        public static bool EQ(this int thisObj, double p)
        {
            return (decimal)thisObj == (decimal)p;
        }


        public static bool EQ(this decimal thisObj, decimal p)
        {
            return (decimal)thisObj == (decimal)p;
        }

        public static bool EQ(this decimal thisObj, decimal? p)
        {
            return (decimal)thisObj == (decimal)p;
        }

        public static string ToNullString(this int? i)
        {
            return i.HasValue ? i.ToString() : "0";
        }

        public static string ToNullString(this decimal? i)
        {
            return i.HasValue ? i.ToString() : "0";
        }

        public static string Format(this decimal? i, string format)
        {
            return ToNullString(i).ToNum().ToString("0.00");
        }

        public static bool NE(this decimal thisObj, decimal p)
        {
            return !thisObj.EQ(p);
        }

        public static bool GT(this decimal thisObj, decimal p)
        {
            return thisObj > p;
        }

        public static bool LT(this decimal thisObj, decimal p)
        {
            return thisObj < p;
        }

        public static bool GE(this decimal thisObj, decimal p)
        {
            if (thisObj.EQ(p))
                return true;

            return thisObj.GT(p);
        }

        public static bool LE(this decimal thisObj, decimal p)
        {
            if (thisObj.EQ(p))
                return true;

            return thisObj.LT(p);
        }

        public static bool IN(this decimal thisObj, params decimal[] values)
        {
            return values.Any(value => thisObj.EQ(value));
        }

        public static bool NotIN(this decimal thisObj, params decimal[] values)
        {
            return !IN(thisObj, values);
        }

        public static bool NE(this int thisObj, double p)
        {
            return !thisObj.EQ(p);
        }

        public static bool NE(this int thisObj, int p)
        {
            return !((int)thisObj == (int)p);
        }

        public static bool GT(this int thisObj, double p)
        {
            return thisObj > p;
        }

        public static bool LT(this int thisObj, double p)
        {
            return thisObj < p;
        }

        public static bool GE(this int obj, double p)
        {
            return obj >= p;
        }

        public static bool LE(this int obj, double p)
        {
            return obj <= p;
        }

        public static bool IN(this int obj, params int[] values)
        {
            return values.Any(value => obj == value);
        }

        public static bool IN(this int? obj, params int[] values)
        {
            return values.Any(value => obj == value);
        }

        public static bool IN(this int obj, params int?[] values)
        {
            return values.Any(value => obj == value);
        }

        public static bool IN(this int? obj, params int?[] values)
        {
            return values.Any(value => obj == value);
        }

        public static bool NotIN(this int obj, params int[] values)
        {
            return !IN(obj, values);
        }

        //--------- Int32-II
        public static double ToDouble(this IEnumerable<int> tObj)
        {
            return Convert.ToDouble(tObj);
        }

        //***************************** Other ***********************************
        public static string ToStr(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetEnumDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetEnumDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}