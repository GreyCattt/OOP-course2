using System;

namespace LabWork
{
    public class MathComponent
    {
        public event EventHandler<DivisionEventArgs> OnIntegerDivision;

        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;

        public int Divide(int a, int b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Ділення на нуль неможливе.");
            }

            int result = a / b;
            int remainder = a % b;

            OnIntegerDivision?.Invoke(this, new DivisionEventArgs(a, b, result, remainder));

            return result;
        }
    }
}