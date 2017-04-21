using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorableTest.Dialogs.Balance
{
    public interface IRememberLastMessageSentToUser
    {
        string LastMessageSentToUser { get; set; }
    }
}
