using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class MyChatClientsManager : IChatClientsManager
    {
        private IChatContract _current;

        public IChatContract Current
        {
            get => _current;
            set => _current = value;
        }

        public bool Add(IChatContract client)
        {
            return true;
        }

        public bool Remove(IChatContract client)
        {
            return true;
        }

        public bool IsExist(IChatContract client)
        {
            return true;
        }
    }
}