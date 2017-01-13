using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CalculationWcfService
{
    public class Ehandler
    {
        interface IExpressionFactory
        {
            void Create(List<Expression> stack);
        }
        private Dictionary<string, IExpressionFactory> dictionary = new Dictionary<string, IExpressionFactory>()
        {
            {"+", new Addfactory() },
            {"-", new Subfactory() },
            {"/", new Devfactory() },
            {"*", new Multfactory() }
        };

        public void Handler(List<Expression> stack ,string token)
        {
            IExpressionFactory factory = dictionary[token];
            if (factory != null)
            {
                factory.Create(stack);
            }
        }
        
        private class Addfactory : IExpressionFactory
        {
            public void Create(List<Expression> stack)
            {
                Expression right = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Expression left = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                stack.Add(Expression.Add(left, right));
            }
        }

        private class Subfactory : IExpressionFactory
        {
            public void Create(List<Expression> stack)
            {
                Expression right = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Expression left = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                stack.Add(Expression.Subtract(left, right));
            }
        }

        private class Devfactory : IExpressionFactory
        {
            public void Create(List<Expression> stack)
            {
                Expression right = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Expression left = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Expression<Func<double>> temp = Expression.Lambda<Func<double>>(right);
                Func<double> res = temp.Compile();
                double result = res();
                if (result != 0)
                    stack.Add(Expression.Divide(left, right));
                else
                    throw new Exception("right cannot be zero");
            }
        }

        private class Multfactory : IExpressionFactory
        {
            public void Create(List<Expression> stack)
            {
                Expression right = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Expression left = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                stack.Add(Expression.Multiply(left, right));
            }
        }
    }
}