using System;
using System.Collections; 
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        List<Book> allBooks = new List<Book>();

        bool keepRunning = true;
        while (keepRunning)
        {
            DisplayMenu();
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n### Завдання 1: Демонстрація класу Book ###");
                    Book sampleBook = ReadBookFromConsole("Введіть дані для книги:");
                    DemoTask1(sampleBook);
                    break;
                
                case "2":
                    allBooks = CreateBookList();
                    break;

                case "3":
                    if (allBooks.Count == 0)
                    {
                        Console.WriteLine("\n[!] Список книг порожній. Будь ласка, спочатку запустіть 'Пункт 2', щоб створити список.");
                    }
                    else
                    {
                        DemoTask2(new List<Book>(allBooks));
                    }
                    break;

                case "4":
                    if (allBooks.Count == 0)
                    {
                        Console.WriteLine("\n[!] Список книг порожній. Будь ласка, спочатку запустіть 'Пункт 2', щоб створити список.");
                    }
                    else
                    {
                        DemoTask3_4(allBooks); 
                    }
                    break;

                case "5":
                    keepRunning = false;
                    Console.WriteLine("\nДякую за роботу! Вихід...");
                    break;

                default:
                    Console.WriteLine("\n[!] Неправильний вибір. Будь ласка, введіть число від 1 до 5.");
                    break;
            }

            if (keepRunning)
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
                Console.ReadKey();
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("======= ГОЛОВНЕ МЕНЮ =======");
        Console.WriteLine("Оберіть опцію:");
        Console.WriteLine("  1. Демонстрація класу Book (Завдання 1)");
        Console.WriteLine("  2. Створити/Перестворити список з 4-х книг (для Завдань 2, 3, 4)");
        Console.WriteLine("  3. Демонстрація List<T> (Завдання 2)");
        Console.WriteLine("  4. Демонстрація Дерева (Завдання 3-4)");
        Console.WriteLine("  5. Вихід");
        Console.Write("Ваш вибір: ");
    }

    private static List<Book> CreateBookList()
    {
        Console.WriteLine("\n### Створення списку книг ###");
        var books = new List<Book>();
        Console.WriteLine("Введіть дані для 4-х книг:");
        for (int i = 0; i < 4; i++)
        {
            books.Add(ReadBookFromConsole($"\nКнига {i + 1}:"));
        }
        Console.WriteLine("\n[i] Список з 4-х книг успішно створено.");
        return books;
    }
    private static void DemoTask1(Book sampleBook)
    {
        Console.WriteLine("\nПочаткова інформація:");
        Console.WriteLine(sampleBook);

        sampleBook.IncreasePrice(10);
        Console.WriteLine("\nПісля підвищення ціни на 10%:");
        Console.WriteLine(sampleBook);

        Console.WriteLine($"\nЗагальна вартість тиражу: {sampleBook.TotalCost:C2}");
        Console.WriteLine(new string('-', 40));
    }

    private static void DemoTask2(List<Book> bookList)
    {
        Console.WriteLine("\n### Завдання 2: Робота з колекцією List<T> ###");
        Console.WriteLine($"\nОтримано список з {bookList.Count} книг.");

        Console.WriteLine("Прохід по List<Book>:");
        foreach (Book book in bookList)
        {
            Console.WriteLine($"\t{book.Title}");
        }

        Console.WriteLine("\n--- Оновлення ---");
        string searchSerial = ReadString("Введіть серійний номер книги для оновлення: ");
        var bookToModify = bookList.Find(b => b.SerialNumber.Equals(searchSerial, StringComparison.OrdinalIgnoreCase));

        if (bookToModify != null)
        {
            decimal newPrice = ReadDecimal($"Введіть нову ціну для '{bookToModify.Title}': ", 0);
            bookToModify.PricePerCopy = newPrice;
            Console.WriteLine($"\nОновлено: Ціна '{bookToModify.Title}' тепер {bookToModify.PricePerCopy:C2}");
        }
        else
        {
            Console.WriteLine($"\nКнигу з серійним номером '{searchSerial}' не знайдено.");
        }

        Console.WriteLine("\n--- Пошук ---");
        string searchListTitle = ReadString("Введіть назву книги для пошуку: ");
        var foundListBook = bookList.Find(b => b.Title.Equals(searchListTitle, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"\nПошук '{searchListTitle}': {(foundListBook != null ? "Знайдено!" : "Не знайдено.")}");

        Console.WriteLine("\n--- Видалення ---");
        string deleteListTitle = ReadString("Введіть назву книги для видалення: ");
        var bookToRemove = bookList.Find(b => b.Title.Equals(deleteListTitle, StringComparison.OrdinalIgnoreCase));

        if (bookToRemove != null)
        {
            bookList.Remove(bookToRemove);
            Console.WriteLine($"\nВидалено '{deleteListTitle}'.");
        }
        else
        {
            Console.WriteLine($"\nКнигу '{deleteListTitle}' не знайдено.");
        }

        Console.WriteLine($"Залишилось елементів у списку: {bookList.Count}");
        
        Console.WriteLine(new string('-', 40));
    }

    private static void DemoTask3_4(List<Book> books)
    {
        Console.WriteLine("\n### Завдання 3: Створення бінарного дерева ###");
        BinaryTree<Book> bookTree = new BinaryTree<Book>();

        foreach (var book in books)
        {
            bookTree.Add(book);
        }

        Console.WriteLine("\n### Завдання 4: Порівняння та Ітератор ###");
        Console.WriteLine("\n--- 2. Демонстрація IComparer<T> (Сортування списку) ---");
        Console.WriteLine("Сортування списку за ціною:");
        
        var sortedListByPrice = new List<Book>(books);
        sortedListByPrice.Sort(new BookPriceComparer());

        foreach (var book in sortedListByPrice)
        {
            Console.WriteLine($"\t{book.PricePerCopy:C2} - {book.Title}");
        }

        Console.WriteLine("\n--- 3. Демонстрація Ітератора (Зворотній обхід / Postorder) ---");
        Console.WriteLine("Обхід дерева у зворотньому порядку (Postorder L-R-N) за допомогою foreach:");

        foreach (Book book in bookTree)
        {
            Console.WriteLine(book);
        }
    }


    private static Book ReadBookFromConsole(string prompt)
    {
        Console.WriteLine(prompt);

        string serialNumber = ReadString("Серійний номер (ISBN): ");
        string title = ReadString("Назва книги: ");
        int year = ReadInt("Рік видання: ", 1000, DateTime.Now.Year);
        decimal price = ReadDecimal("Ціна за копію: ", 0);
        int copies = ReadInt("Кількість копій: ", 0);

        return new Book(serialNumber, title, year, price, copies);
    }
    
    private static string ReadString(string prompt)
    {
        string input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Введення не може бути порожнім. Спробуйте ще раз.");
            }
        } while (string.IsNullOrWhiteSpace(input));
        return input;
    }
    private static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
            {
                return value;
            }
            Console.WriteLine($"Будь ласка, введіть правильне ціле число (між {min} і {max}).");
        }
    }
    
    private static decimal ReadDecimal(string prompt, decimal min = decimal.MinValue)
    {
        decimal value;
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.CurrentCulture, out value) && value >= min)
            {
                return value;
            }
            Console.WriteLine($"Будь ласка, введіть правильну ціну (більше або рівну {min}).");
        }
    }
}