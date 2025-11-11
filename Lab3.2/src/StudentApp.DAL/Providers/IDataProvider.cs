using System.Collections.Generic;

namespace StudentApp.DAL.Providers
{
    public interface IDataProvider<T> where T : class
    {
        void Write(IEnumerable<T> data, string filePath);
        IEnumerable<T> Read(string filePath);
    }
}