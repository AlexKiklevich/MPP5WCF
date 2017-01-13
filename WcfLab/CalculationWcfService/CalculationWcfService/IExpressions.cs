using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CalculationWcfService
{
    [ServiceContract]
    public interface IExpressions
    {
        [OperationContract]
        string CalculateExpression(string expression);

        [OperationContract]
        void Set(double a);

        [OperationContract]
        void Add(double a);

        [OperationContract]
        void Substract(double a);

        [OperationContract]
        void Multiply(double a);

        [OperationContract]
        void Divide(double a);

        [OperationContract]
        double Result();
    }
}
