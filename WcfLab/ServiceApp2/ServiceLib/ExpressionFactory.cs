using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLib
{
    class ExpressionFactory
    {
        private delegate Expression OperationDelegate(Expression left, Expression right);

        private readonly Dictionary<string, OperationDelegate> _dictionary = new Dictionary<string, OperationDelegate>()
        {
            {"+", (left, right) => ExpressionAdd(left, right)},
            {"-", (left, right) => ExpressionSubstract(left, right)},
            {"*", (left, right) => ExpressionMultiply(left, right)},
            {
                "/", (left, right) =>
                {
                    Expression<Func<double>> temp = Expression.Lambda<Func<double>>(right);
                    Func<double> res = temp.Compile();
                    double result = res();
                    if (Math.Abs(result) > 0.001)
                        return ExpressionDivide(left, right);
                    throw new ArgumentException("Can't divide by zero");
                }
            },
        };

        public void Handle(Stack<Expression> expressionStack, string token)
        {
            OperationDelegate operation = _dictionary[token];

            Expression right = expressionStack.Pop();
            Expression left = expressionStack.Pop();
            //try
            //{
            //    left = expressionStack.Pop();
            //}
            //catch (Exception)
            //{
            //    if (token == "-")
            //    {
            //        left = Expression.Constant((double)0);
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            expressionStack.Push(operation(left, right));
        }

        private static Expression ExpressionAdd(Expression left, Expression right)
        {
            Calculator lib = new Calculator();
            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Set",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    left)).Compile()();

            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Add",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    right)).Compile()();

            var mi = typeof(Calculator).GetMethod("Result", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            MethodCallExpression expr = Expression.Call(
                Expression.Constant(lib),
                mi);
            return expr;
        }

        private static Expression ExpressionSubstract(Expression left, Expression right)
        {
            Calculator lib = new Calculator();
            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Set",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    left)).Compile()();

            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Substract",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    right)).Compile()();

            var mi = typeof(Calculator).GetMethod("Result", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            MethodCallExpression expr = Expression.Call(
                Expression.Constant(lib),
                mi);
            return expr;
        }

        private static Expression ExpressionMultiply(Expression left, Expression right)
        {
            Calculator lib = new Calculator();
            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Set",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    left)).Compile()();

            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Multiply",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    right)).Compile()();

            var mi = typeof(Calculator).GetMethod("Result", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            MethodCallExpression expr = Expression.Call(
                Expression.Constant(lib),
                mi);
            return expr;
        }

        private static Expression ExpressionDivide(Expression left, Expression right)
        {
            Calculator lib = new Calculator();
            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Set",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(double) },
                        null),
                    left)).Compile()();

            Expression.Lambda<Action>(
                Expression.Call(
                    Expression.Constant(lib),
                    typeof(Calculator).GetMethod(
                        "Divide",
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new Type[] { typeof(int) },
                        null),
                    right)).Compile()();

            var mi = typeof(Calculator).GetMethod("Result", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            MethodCallExpression expr = Expression.Call(
                Expression.Constant(lib),
                mi);
            return expr;
        }
    }
}
