using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace ServiceLib
{
    /// <summary>
    /// Interface for WCF Calculator contracts
    /// </summary>
    [ServiceContract]
    public interface ICalculator
    {
        /// <summary>
        /// Compiles the expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        [OperationContract]
        string CompileExpression(string expression);

        /// <summary>
        /// Sets value to the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        [OperationContract]
        void Set(double a);

        /// <summary>
        /// Adds the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        [OperationContract]
        void Add(double a);

        /// <summary>
        /// Multiplies the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        [OperationContract]
        void Multiply(double a);

        /// <summary>
        /// Substracts the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        [OperationContract]
        void Substract(double a);

        /// <summary>
        /// Divides to the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        [OperationContract]
        void Divide(double a);

        /// <summary>
        /// Resets value.
        /// </summary>
        [OperationContract]
        void Reset();

        /// <summary>
        /// Returns result of the calculation.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        double Result();
    }
}
