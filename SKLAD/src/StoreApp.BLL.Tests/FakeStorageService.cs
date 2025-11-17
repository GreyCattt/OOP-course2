using StoreApp.DAL;

namespace StoreApp.BLL.Tests
{
    public class FakeStorageService : IStorageService
    {
        public DataContext Context { get; private set; }

        public FakeStorageService()
        {
            Context = new DataContext();
        }

        public DataContext LoadContext()
        {
            return Context;
        }

        public void SaveContext(DataContext context)
        {
   
        }
    }
}