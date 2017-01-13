using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLib
{
    public class Calculator : ICalculator
    {
        [DllImport(@"C:\Users\Sasha\Documents\Visual Studio 2015\Projects\WcfLab\CalculationDll\Debug\CalculationDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Add(double a, double b);

        [DllImport(@"C:\Users\Sasha\Documents\Visual Studio 2015\Projects\WcfLab\CalculationDll\Debug\CalculationDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Substract(double a, double b);

        [DllImport(@"C:\Users\Sasha\Documents\Visual Studio 2015\Projects\WcfLab\CalculationDll\Debug\CalculationDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Multiply(double a, double b);

        [DllImport(@"C:\Users\Sasha\Documents\Visual Studio 2015\Projects\WcfLab\CalculationDll\Debug\CalculationDll.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern double Divide(double a, double b);

        private double _result = 0;

        public double Result() => _result;

        public string CompileExpression(string expression)
        {
            List<string> tokens = ParseString(expression);
            var rpn = CreateRpn(tokens);
            try
            {
            Expression<Func<double>> lambdaExpression = Expression.Lambda<Func<double>>(CreateExpressionTree(rpn));
            Func<double> calculateExpression = lambdaExpression.Compile();
            return calculateExpression().ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return "Error while compiling expression";
            }
        }

        private List<string> ParseString(string expression)
        {
            List<string> result = new List<string>();

            int j = -1;
            for (int i = 0; i < expression.Length; i++)
            {
                if (!Char.IsNumber(expression[i]))
                {
                    j++;
                    result.Add(String.Empty);
                    result[j] = expression[i].ToString();
                }
                else
                {
                    if (i == 0 || !Char.IsNumber(expression[i - 1]))
                    {
                        j++;
                        result.Add(String.Empty);
                        result[j] += expression[i];
                    }
                    else
                    {
                        result[j] += expression[i];
                    }
                }
            }
            return result;
        }

        public void Set(double a)
        {
            _result = a;
        }

        public void Add(double a)
        {
            _result = Add(_result, a);
        }

        public void Multiply(double a)
        {
            _result = Multiply(_result, a);
        }

        public void Substract(double a)
        {
            _result = Substract(_result, a);
        }

        public void Divide(double a)
        {
            if (Math.Abs(a) < 0.001) throw new ArgumentOutOfRangeException(nameof(a));
            _result = Divide(_result, a);
        }

        public void Reset()
        {
            _result = 0;
        }

        private static Expression CreateExpressionTree(List<string> tokens)
        {
            ExpressionFactory handler = new ExpressionFactory();
            Stack<Expression> expressionStack = new Stack<Expression>();
            foreach (var token in tokens)
            {
                double temp;
                if (double.TryParse(token.ToString(), out temp))
                {
                    expressionStack.Push(Expression.Constant(temp));
                }
                else
                {
                    handler.Handle(expressionStack, token);
                }
            }
            return expressionStack.Peek();
        }

        public static List<string> CreateRpn(List<string> tokens)
        {
            List<string> result = new List<string>();
            List<string> stack = new List<string>();
            List<int> priority = new List<int>();
            for (int i = 0; i < tokens.Count(); i++)
            {
                double tempIntVar;
                if (double.TryParse(tokens[i], out tempIntVar))
                {
                    bool canBeNegative = false;
                    if (i > 1)
                    {
                        canBeNegative = !double.TryParse(tokens[i - 2], out tempIntVar);
                    }

                    if ((i == 1 && tokens[i-1] == "-") || (canBeNegative && tokens[i - 1] == "-"))
                    {
                        stack.RemoveAt(stack.Count - 1);
                        priority.RemoveAt(priority.Count-1);
                        result.Add("-" + tokens[i]);
                    }
                    else 
                        result.Add(tokens[i]);
                }
                else
                {
                    switch (tokens[i])
                    {
                        case "*":
                        case "/":
                            {
                                while ((priority.Count != 0) && (priority[priority.Count - 1] >= 2))
                                {
                                    result.Add(stack[stack.Count - 1]);
                                    stack.RemoveAt(stack.Count - 1);
                                    priority.RemoveAt(priority.Count - 1);
                                }
                                stack.Add(tokens[i]);
                                priority.Add(2);
                                break;
                            }
                        case "+":
                        case "-":
                            {

                                while ((priority.Count != 0) && (priority[priority.Count - 1] >= 1))
                                {
                                    result.Add(stack[stack.Count - 1]);
                                    stack.RemoveAt(stack.Count - 1);
                                    priority.RemoveAt(priority.Count - 1);
                                }
                                stack.Add(tokens[i]);
                                priority.Add(1);
                                break;
                            }
                        case "(":
                            {
                                stack.Add(tokens[i]);
                                priority.Add(0);
                                break;
                            }
                        case ")":
                            {
                                while (stack[stack.Count - 1] != "(")
                                {
                                    result.Add(stack[stack.Count - 1]);
                                    stack.RemoveAt(stack.Count - 1);
                                    priority.RemoveAt(priority.Count - 1);
                                }
                                stack.RemoveAt(stack.Count - 1);
                                priority.RemoveAt(priority.Count - 1);
                                break;
                            }
                    }
                }
            }
            while (stack.Count != 0)
            {
                result.Add(stack[stack.Count - 1]);
                stack.RemoveAt(stack.Count - 1);
                priority.RemoveAt(priority.Count - 1);
            }
            return result;
        }
    }
}
