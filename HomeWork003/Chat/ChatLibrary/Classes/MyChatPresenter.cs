using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class MyChatPresenter
    {
        private IChatView _view;
        private ServiceHost _host;

        public MyChatPresenter(IChatView view)
        {
            try
            {
                _view = view;
                _host = new ServiceHost(typeof(MyChatServer));
                _host.Open();
                _view.IncomingMainChatMessage += _view_IncomingMainChatMessage;
                _view.IncomingPersonalChatMessage += _view_IncomingPersonalChatMessage;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void _view_IncomingPersonalChatMessage(string message)
        {
            
        }

        private void _view_IncomingMainChatMessage(string message)
        {

        }
    }


}
