using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CalculationWcfService
{
    
    public class Calculator : IExpressions
    {
        [DllImport(@"CalculationDll.dll", CallingConvention = CallingConvention.StdCall)]
        static extern double Add(double a, double b);
        [DllImport(@"CalculationDll.dll", CallingConvention = CallingConvention.StdCall)]
        static extern double Substract(double a, double b);
        [DllImport(@"CalculationDll.dll", CallingConvention = CallingConvention.StdCall)]
        static extern double Multiply(double a, double b);
        [DllImport(@"CalculationDll.dll", CallingConvention = CallingConvention.StdCall)]
        static extern double Divide(double a, double b);
        

        public string CalculateExpression(string expression)
        {
            string[] tokens = ExpressionsUtils.ParseExpression(expression);
            List<string> rpn = ExpressionsUtils.CreateRpn(tokens);
            Expression<Func<double>> lambda = Expression.Lambda<Func<double>>(GetExpressionTree(rpn));
            Func<double> myDelegate = lambda.Compile();
            double result = myDelegate();
            return result.ToString();
        }

        private Expression GetExpressionTree(List<string> rpn)
        {
            Ehandler exprhandler = new Ehandler();
            List<Expression> expressionstack = new List<Expression>();
            foreach (string token in rpn)
            {
                double buf;
                if(Double.TryParse(token, out buf))
                {
                    expressionstack.Add(Expression.Constant(buf));
                }
                else
                {
                    exprhandler.Handler(expressionstack, token);
                }
            }
            return expressionstack[0];
        }

        private double result = 0;
        public void Set(double a)
        {
            result = a;
        }

        public void Add(double a)
        {
            result = Add(result, a);
        }

        public void Substract(double a)
        {
            result = Substract(result, a);
        }

        public void Multiply(double a)
        {
            result = Multiply(result, a);
        }

        public void Divide(double a)
        {
            result = Divide(result, a);
        }

        public double Result()
        {
            return result;
        }
    }
}