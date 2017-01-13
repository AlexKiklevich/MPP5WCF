using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ServiceReference1.CalculatorClient("NetTcpBinding_ICalculator");
            string exspression = "";
            string result = "";
            double res = 0;
            while (true)
            {
                client = new ServiceReference1.CalculatorClient("NetTcpBinding_ICalculator");
                try
                {
                    Console.Write("Input your exspression(Exit input \"exit\"): ");
                    exspression = Console.ReadLine();

                    if (exspression == "exit")
                        break;

                    exspression = exspression.Replace(" ", "");
                    foreach (var symbol in exspression)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(symbol.ToString(), @"^[0-9,\-,+,*,/,(,)]$") == false)
                        {
                            result = "Banned symbols in string";
                        }
                        else
                            result = client.CompileExpression(exspression);
                    }
                    
                    if (Double.TryParse(result, out res))
                        Console.WriteLine("Result = " + res);
                    else
                        Console.WriteLine("Error: " + result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Введены некорректные данные");
                }
            }
            client.Close();
        }
    }
}
