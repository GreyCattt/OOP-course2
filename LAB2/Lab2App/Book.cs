using System;

public class Book : IComparable<Book>
{
    public string SerialNumber { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public decimal PricePerCopy { get; set; }
    public int CopiesCount { get; set; }

 
    public Book(string serial, string title, int year, decimal price, int count)
    {
        SerialNumber = serial;
        Title = title;
        PublicationYear = year;
        PricePerCopy = price;
        CopiesCount = count;
    }

    public decimal TotalCost => PricePerCopy * CopiesCount;

    public void IncreasePrice(double percentage)
    {
        if (percentage < 0) return;
        PricePerCopy *= (1 + (decimal)percentage / 100);
    }

    public override string ToString()
    {
        return $"'{Title}' ({SerialNumber}) - {PublicationYear}. {CopiesCount} шт. @ {PricePerCopy:C2} кожна. Всього: {TotalCost:C2}";
    }
    public int CompareTo(Book other)
    {
        if (other == null) return 1;
        return string.Compare(this.SerialNumber, other.SerialNumber, StringComparison.Ordinal);
    }
}