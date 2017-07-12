namespace ChatLibrary.Interfaces
{
    public interface IManagedChat : IChatContract
    {
        IChatClientsManager ClientsManager { get; set; }
    }
}