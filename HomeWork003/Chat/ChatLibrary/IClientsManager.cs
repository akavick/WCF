namespace ChatLibrary
{
    public interface IClientsManager
    {
        bool Add(IChatClient client);
        bool Remove(IChatClient client);
    }
}