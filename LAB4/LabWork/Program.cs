namespace LabWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Завдання 1: Лямбда-вираз ---");

            CharCountDelegate countOccurrences = (text, charToFind) =>
            {
                if (string.IsNullOrEmpty(text)) return 0;
                
                int count = 0;
                foreach (char c in text)
                {
                    if (c == charToFind) count++;
                }
                return count;
            };

            Console.Write("Введіть рядок: ");
            string testStr = Console.ReadLine();
            Console.Write("Введіть символ для пошуку: ");
            char testChar = Console.ReadKey().KeyChar;
            Console.WriteLine();
            int count = countOccurrences(testStr, testChar);

            Console.WriteLine($"Рядок: {testStr}");
            Console.WriteLine($"Символ '{testChar}' знайдено разів: {count}");
            Console.WriteLine(new string('-', 30));

            Console.WriteLine("\n--- Завдання 3: Обробка подій ---");

            MathComponent mathComp = new MathComponent();

            mathComp.OnIntegerDivision += Handler_OnDivision;

            try
            {
                Console.Write("Введіть ділене (ціле число): ");
                int dividend = int.Parse(Console.ReadLine());
                Console.Write("Введіть дільник (ціле число): ");
                int divisor = int.Parse(Console.ReadLine());
                Console.WriteLine($"Виконуємо ділення {dividend} на {divisor}...");
                int result = mathComp.Divide(dividend, divisor);
                Console.WriteLine($"Результат у Main: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            Console.ReadKey();
        }

        private static void Handler_OnDivision(object sender, DivisionEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" >> [ПОДІЯ] Відбулося цілочисельне ділення!");
            Console.WriteLine($" >> Деталі: {e.Dividend} / {e.Divisor} = {e.Result}, Остача: {e.Remainder}");
            Console.ResetColor();
        }
    }
}