using TadesApi.CoreHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TadesApi.Core.Extensions
{

    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> LikeFilter<T>(this Expression<Func<T, bool>> source, Expression<Func<T, string>> expr, string terms)
        {
            var paramExpr = expr.Parameters.First();
            var memExpr = expr.Body;

            if (terms == null || expr == null)
                return source;
            if (terms.StartsWith('*'))
            {
                var likeValue = terms.Remove(0, 1);
                Expression<Func<string>> valExpr = () => likeValue;
                var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                    new[] { typeof(string) }), valExpr.Body);
                return Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);
            }
            else if (terms.EndsWith('%'))
            {
                var likeValue = terms.Remove(terms.Length - 1);
                Expression<Func<string>> valExpr = () => likeValue;
                var startsExpr = Expression.Call(memExpr, typeof(String).GetMethod("StartsWith",
                    new[] { typeof(string) }), valExpr.Body);
                return Expression.Lambda<Func<T, bool>>(startsExpr, paramExpr);
            }
            else
            {
                var likeValue = terms;
                Expression<Func<string>> valExpr = () => likeValue;
                var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                    new[] { typeof(string) }), valExpr.Body);

                return Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);


            }
        }
        public static Expression<Func<T, bool>> LikeFilter<T>(this Expression<Func<T, bool>> source, string terms, params Expression<Func<T, string>>[] exprs)
        {

            foreach (var expr in exprs)
            {

                var paramExpr = expr.Parameters.First();
                var memExpr = expr.Body;

                if (terms == null || expr == null)
                    continue;
                if (terms.StartsWith('*'))
                {

                    var likeValue = terms.Remove(0, 1);
                    Expression<Func<string>> valExpr = () => likeValue;
                    var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);

                    source = source.Or(exp);
                }
                else if (terms.EndsWith('%'))
                {
                    var likeValue = terms.Remove(terms.Length - 1);
                    Expression<Func<string>> valExpr = () => likeValue;
                    var startsExpr = Expression.Call(memExpr, typeof(String).GetMethod("StartsWith",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(startsExpr, paramExpr);
                    source = source.Or(exp);
                }
                else
                {

                    var likeValue = terms;
                    Expression<Func<string>> valExpr = () => likeValue;
                    var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);

                    source = source.Or(exp);
                }
            }


            return source;


        }
        public static IQueryable<T> LikeFilter<T>(this IQueryable<T> query, string terms, params Expression<Func<T, string>>[] exprs)
        {

            var predicate = PredicateBuilder.False<T>();

            foreach (var expr in exprs)
            {

                var paramExpr = expr.Parameters.First();
                var memExpr = expr.Body;

                if (terms == null || expr == null)
                    continue;
                if (terms.StartsWith('*'))
                {

                    var likeValue = terms.Remove(0, 1);
                    Expression<Func<string>> valExpr = () => likeValue;
                    var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);

                    predicate = predicate.Or(exp);
                }
                else if (terms.EndsWith('%'))
                {
                    var likeValue = terms.Remove(terms.Length - 1);
                    Expression<Func<string>> valExpr = () => likeValue;
                    var startsExpr = Expression.Call(memExpr, typeof(String).GetMethod("StartsWith",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(startsExpr, paramExpr);

                    predicate = predicate.Or(exp);
                }
                else
                {
                    var likeValue = terms;
                    Expression<Func<string>> valExpr = () => likeValue;
                    var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                        new[] { typeof(string) }), valExpr.Body);
                    var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);
                    predicate = predicate.Or(exp);

                }
            }


            return query.Where(predicate);


        }
        public static IQueryable<T> LikeFilter<T>(this IQueryable<T> source, Expression<Func<T, string>> expr, string terms)
        {
            var predicate = PredicateBuilder.False<T>();

            var paramExpr = expr.Parameters.First();
            var memExpr = expr.Body;

            if (terms == null || expr == null)
                return source;
            if (terms.StartsWith('*'))
            {

                var likeValue = terms.Remove(0, 1);
                Expression<Func<string>> valExpr = () => likeValue;
                var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                    new[] { typeof(string) }), valExpr.Body);
                var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);

                predicate = predicate.Or(exp);
            }
            else if (terms.EndsWith('%'))
            {
                var likeValue = terms.Remove(terms.Length - 1);
                Expression<Func<string>> valExpr = () => likeValue;
                var startsExpr = Expression.Call(memExpr, typeof(String).GetMethod("StartsWith",
                    new[] { typeof(string) }), valExpr.Body);
                var exp = Expression.Lambda<Func<T, bool>>(startsExpr, paramExpr);
                predicate = predicate.Or(exp);

            }
            else
            {
                var likeValue = terms;
                Expression<Func<string>> valExpr = () => likeValue;
                var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
                    new[] { typeof(string) }), valExpr.Body);
                var exp = Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);
                predicate = predicate.Or(exp);

            }


            return source.Where(predicate);


        }

    }
}


