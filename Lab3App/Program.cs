using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Book[] originalBooks = CreateInitialBooks();
        
        ISerializationStrategy defaultStrategy = new JsonSerializationStrategy();
        
        RunPart2And3_ArrayDemo(originalBooks, defaultStrategy);

        RunPart4_CollectionDemo(originalBooks, defaultStrategy);
        RunPart5_StrategyDemo(originalBooks);
    }
    

    private static void RunPart2And3_ArrayDemo(Book[] originalBooks, ISerializationStrategy strategy)
    {
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"### Частини 2 і 3: Масив (з використанням {strategy.Name}) ###");
        Console.WriteLine(new string('=', 60));

        PrintBooks("1. Оригінальний масив об'єктів", originalBooks);

        string fileName = "books_array.dat";

        strategy.Serialize(originalBooks, fileName);
        Console.WriteLine($"\n2. Масив успішно серіалізовано у файл: {fileName}");

        Book[] deserializedBooks = strategy.Deserialize<Book[]>(fileName);

        PrintBooks("\n3. Новий десеріалізований масив", deserializedBooks);
    }

    private static void RunPart4_CollectionDemo(Book[] originalBooks, ISerializationStrategy strategy)
    {
        Console.WriteLine("\n\n" + new string('=', 60));
        Console.WriteLine($"### Частина 4: Колекція List<Book> (з {strategy.Name}) ###");
        Console.WriteLine(new string('=', 60));

        List<Book> originalList = new List<Book>(originalBooks);
        originalList.Add(new Book("978-0137081073", "The C++ Programming Language", 2013, 69.99m, 50));

        PrintBooks("1. Оригінальна колекція List<Book>", originalList);

        string fileName = "books_list.dat";

        strategy.Serialize(originalList, fileName);
        Console.WriteLine($"\n2. Колекцію List<Book> серіалізовано у файл: {fileName}");


        List<Book> deserializedList = strategy.Deserialize<List<Book>>(fileName);
        PrintBooks("\n3. Нова десеріалізована колекція List<Book>", deserializedList);
    }

    private static void RunPart5_StrategyDemo(Book[] books)
    {
        Console.WriteLine("\n\n" + new string('=', 60));
        Console.WriteLine("### Частина 5: Демонстрація різних стратегій серіалізації ###");
        Console.WriteLine(new string('=', 60));

        List<ISerializationStrategy> strategies = new List<ISerializationStrategy>
        {
            new JsonSerializationStrategy(),
            new XmlSerializationStrategy(),
            new BinarySerializationStrategy(),

        };

        foreach (var strategy in strategies)
        {
            Console.WriteLine($"\n--- Тестування стратегії: {strategy.Name} ---");
            string fileName = $"books_demo.{strategy.Name.Split(' ')[0].ToLower()}";

            try
            {
                strategy.Serialize(books, fileName);
                Console.WriteLine($"Успішно серіалізовано в: {fileName}");
                
                Book[] deserializedBooks = strategy.Deserialize<Book[]>(fileName);
                Console.WriteLine("Успішно десеріалізовано. Перевірка першого об'єкта:");
                PrintBooks("Результат", deserializedBooks.Take(1));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ПОМИЛКА стратегії {strategy.Name}: {ex.Message}");
                Console.ResetColor();
            }
        }
    }


    private static Book[] CreateInitialBooks()
    {
        return new Book[]
        {
            new Book("978-0321765723", "The C# Programming Language", 2010, 59.99m, 100),
            new Book("978-0132354181", "Clean Code", 2008, 44.95m, 250),
            new Book("978-0201633610", "Design Patterns", 1994, 54.99m, 150),
            new Book("978-0735619678", "Code Complete 2", 2004, 49.99m, 200)
        };
    }

    private static void PrintBooks<T>(string title, IEnumerable<T> books)
    {
        Console.WriteLine($"--- {title} ---");
        foreach (var book in books)
        {
            Console.WriteLine(book);
            Console.WriteLine(new string('-', 20));
        }
    }
}