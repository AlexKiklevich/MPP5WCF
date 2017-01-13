#include <stdexcept>
extern "C" __declspec(dllexport)
double Add(double argument1, double argument2)
{
	return argument1 + argument2;
}
extern "C" __declspec(dllexport)
double  Substract(double argument1, double argument2)
{
	return argument1 - argument2;
}
extern "C" __declspec(dllexport)
double Multiply(double argument1, double argument2)
{
	return argument1 * argument2;
}
extern "C" __declspec(dllexport)
double Divide(double argument1, double argument2)
{
	if (argument2 == 0)
		throw std::invalid_argument("argument2 cannot be zero!");
	return argument1 / argument2;
}