namespace LabWork
{
    public class DivisionEventArgs : EventArgs
    {
        public int Dividend { get; } 
        public int Divisor { get; } 
        public int Result { get; }  
        public int Remainder { get; }

        public DivisionEventArgs(int dividend, int divisor, int result, int remainder)
        {
            Dividend = dividend;
            Divisor = divisor;
            Result = result;
            Remainder = remainder;
        }
    }
}