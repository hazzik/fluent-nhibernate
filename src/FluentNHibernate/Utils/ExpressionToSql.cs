using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentNHibernate.Utils
{
    /// <summary>
    /// Converts an expression to a best guess SQL string
    /// </summary>
    public static class ExpressionToSql
    {
        /// <summary>
        /// Converts a Func expression to a best guess SQL string
        /// </summary>
        public static string Convert<T>(Expression<Func<T, object>> expression)
        {
            return Convert<T>((LambdaExpression)expression);
        }

        /// <summary>
        /// Converts a boolean Func expression to a best guess SQL string
        /// </summary>
        public static string Convert<T>(Expression<Func<T, bool>> expression)
        {
            Expression body = expression.Body;
            if (body is BinaryExpression)
            {
                return Convert<T>((BinaryExpression)body);
            }
            if (body.Type == typeof(bool))
            {
                string convert = Convert<T>((LambdaExpression)expression);
                if (body is UnaryExpression)
                    return string.Format("{0} = {1}", convert, Convert(false));
                return string.Format("{0} = {1}", convert, Convert(true));
            }
            throw new InvalidOperationException("Unable to convert expression to SQL");
        }

        static string Convert<T>(LambdaExpression expression)
        {
            return Convert<T>(expression, expression.Body);
        }

        static string Convert<T>(LambdaExpression expression, Expression body)
        {
            if (body is ConstantExpression)
            {
                return Convert((ConstantExpression)body);
            }
            if (body is UnaryExpression)
            {
                return Convert<T>(expression, ((UnaryExpression)body).Operand);
            }
            if (body is MemberExpression)
            {
                return Convert<T>(expression, (MemberExpression)body);
            }
            if (body is MethodCallExpression)
            {
                return Convert<T>(expression, (MethodCallExpression)body);
            }

            throw new InvalidOperationException("Unable to convert expression to SQL");
        }

        static string Convert(ConstantExpression expression)
        {
            if (expression.Type.IsEnum)
                return Convert((int)expression.Value);

            return Convert(expression.Value);
        }

        static string Convert<T>(LambdaExpression expression, UnaryExpression body)
        {
            return Convert<T>(expression, body.Operand);
        }

        static string Convert<T>(LambdaExpression expression, MemberExpression body)
        {
            // TODO: should really do something about conventions and overridden names here
            MemberInfo member = body.Member;

            if (member.DeclaringType == typeof(T))
                return member.Name;

            return Eval<T>(expression);
        }

        static string Convert<T>(LambdaExpression expression, MethodCallExpression body)
        {
            // TODO: should really do something about conventions and overridden names here
            MethodInfo method = body.Method;

            if (method.DeclaringType == typeof(T))
                return method.Name;

            return Eval<T>(expression);
        }

        static string Eval<T>(LambdaExpression expression)
        {
            // try get value of lambda, hoping it's just a direct value return or local reference
            Delegate compiledExpression = expression.Compile();
            object value = compiledExpression.DynamicInvoke(default(T)); // give it null/default because a value will not need anything
            return Convert(value);
        }

        static Expression<Func<T, object>> CreateExpression<T>(Expression body)
        {
            Expression expression = body;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            if (expression.Type.IsValueType)
                expression = Expression.Convert(expression, typeof(object));

            return Expression.Lambda<Func<T, object>>(expression, parameter);
        }

        static string Convert<T>(BinaryExpression expression)
        {
            string left = Convert(CreateExpression<T>(expression.Left));
            string right = Convert(CreateExpression<T>(expression.Right));
            string op;

            switch (expression.NodeType)
            {
                default:
                case ExpressionType.Equal:
                    op = "=";
                    break;
                case ExpressionType.GreaterThan:
                    op = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    op = ">=";
                    break;
                case ExpressionType.LessThan:
                    op = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    op = "<=";
                    break;
                case ExpressionType.NotEqual:
                    op = "!=";
                    break;
            }

            return string.Format("{0} {1} {2}", left, op, right);
        }

        static string Convert(object value)
        {
            if (value is string)
                return string.Format("'{0}'", value);
            if (value is bool)
                return (bool)value ? "1" : "0";

            return value.ToString();
        }
    }
}