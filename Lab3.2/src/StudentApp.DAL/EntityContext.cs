using StudentApp.DAL.Providers;
using System.Collections.Generic;

namespace StudentApp.DAL
{
   
    public class EntityContext<T> where T : class
    {
        private readonly IDataProvider<T> _provider;
        private readonly string _filePath;

        public EntityContext(string filePath, IDataProvider<T> provider)
        {
            _filePath = filePath;
            _provider = provider;
        }

        public IEnumerable<T> LoadData()
        {
            return _provider.Read(_filePath);
        }

        public void SaveData(IEnumerable<T> data)
        {
            _provider.Write(data, _filePath);
        }
    }
}