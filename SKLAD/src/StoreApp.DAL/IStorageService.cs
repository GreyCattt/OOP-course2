namespace StoreApp.DAL
{
    public interface IStorageService
    {
        DataContext LoadContext();
        void SaveContext(DataContext context);
    }
}