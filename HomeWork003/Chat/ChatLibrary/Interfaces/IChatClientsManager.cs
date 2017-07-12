namespace ChatLibrary.Interfaces
{
    public interface IChatClientsManager
    {
        IChatContract Current { get; set; }
        bool Add(IChatContract client);
        bool Remove(IChatContract client);
        bool IsExist(IChatContract client);
    }
}