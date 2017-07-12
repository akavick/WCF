using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class MyChatPresenter : IChatPresenter
    {
        private IChatContract _chat;
        private IChatView _view;

        public IChatContract Chat
        {
            get { return _chat; }
            set
            {
                _chat = value;

            }
        }

        public IChatView View
        {
            get => _view;
            set
            {
                _view = value;
                _view.IncomingMainChatMessage += _view_IncomingMainChatMessage;
                _view.IncomingPersonalChatMessage += _view_IncomingPersonalChatMessage;
            }
        }

        private void _view_IncomingPersonalChatMessage(string obj)
        {
            
        }

        private void _view_IncomingMainChatMessage(string obj)
        {
            
        }
    }


}
