using System;
using System.Runtime.Serialization;

/// <summary>
/// Атрибут [Serializable] необхідний для BinaryFormatter.
/// </summary>
[Serializable]
public class Book : ISerializable 
{
    // Властивості
    public string SerialNumber { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public decimal Price { get; set; }
    public int Copies { get; set; }

    // Обчислювана властивість
    public decimal TotalCost => Price * Copies;

    // Метод
    public void IncreasePrice(double percentage)
    {
        Price *= (1 + (decimal)percentage / 100.0m);
    }

    // Метод (перевизначення ToString)
    public override string ToString()
    {
        return $"'{Title}' (S/N: {SerialNumber})\n" +
               $"  Рік: {PublicationYear}, Ціна: {Price:C}, Кількість: {Copies}\n" +
               $"  Загальна вартість тиражу: {TotalCost:C}";
    }

    // --- Конструктори ---

    /// <summary>
    /// Публічний конструктор без параметрів (обов'язковий для XmlSerializer)
    /// </summary>
    public Book()
    {
        SerialNumber = "N/A";
        Title = "N/A";
    }

    /// <summary>
    /// Основний конструктор
    /// </summary>
    public Book(string serial, string title, int year, decimal price, int copies)
    {
        SerialNumber = serial;
        Title = title;
        PublicationYear = year;
        Price = price;
        Copies = copies;
    }

    // --- Реалізація ISerializable (Користувацька серіалізація) ---

    /// <summary>
    /// Метод для серіалізації (викликається BinaryFormatter)
    /// </summary>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Serial", SerialNumber);
        info.AddValue("Title", Title);
        info.AddValue("Year", PublicationYear);
        info.AddValue("Price", Price);
        info.AddValue("Copies", Copies);
        info.AddValue("SerializationTime", DateTime.Now);
    }

    protected Book(SerializationInfo info, StreamingContext context)
    {
        SerialNumber = info.GetString("Serial");
        Title = info.GetString("Title");
        PublicationYear = info.GetInt32("Year");
        Price = info.GetDecimal("Price");
        Copies = info.GetInt32("Copies");
        DateTime time = info.GetDateTime("SerializationTime");
        Console.WriteLine($"... (Користувацька десеріалізація: '{Title}' було збережено о {time:T}) ...");
    }
}