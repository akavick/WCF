using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary.Interfaces
{
    public interface IChatPresenter
    {
        IChatContract Chat { get; set; }
        IChatView View { get; set; }
    }
}
