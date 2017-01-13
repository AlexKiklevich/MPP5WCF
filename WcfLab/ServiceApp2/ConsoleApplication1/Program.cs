using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServiceLib;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new ServiceHost(typeof(ServiceLib.Calculator)))
            {
                service.Open();
                Console.WriteLine("Service started!");
                Console.ReadKey();
            }
        }
    }
}
