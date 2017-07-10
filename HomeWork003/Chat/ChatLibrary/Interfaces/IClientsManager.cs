namespace ChatLibrary.Interfaces
{
    public interface IClientsManager
    {
        IChat Current { get; }
        bool Add(IChat client);
        bool Remove(IChat client);
        bool IsExist(IChat client);
    }
}