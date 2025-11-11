/// <summary>
/// Абстракція (інтерфейс), що описує будь-яку стратегію серіалізації.
/// Це дозволяє легко додавати нові формати (OCP).
/// </summary>
public interface ISerializationStrategy
{
    /// <summary>
    /// Назва стратегії (для виведення в консоль).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Серіалізує об'єкт у файл.
    /// </summary>
    void Serialize<T>(T obj, string filePath);

    /// <summary>
    /// Десеріалізує об'єкт з файлу.
    /// </summary>
    T Deserialize<T>(string filePath);
}