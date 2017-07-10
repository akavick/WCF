namespace ChatLibrary.Interfaces
{
    public interface IClientsManager
    {
        IChatClient Current { get; }
        bool Add(IChatClient client);
        bool Remove(IChatClient client);
    }
}