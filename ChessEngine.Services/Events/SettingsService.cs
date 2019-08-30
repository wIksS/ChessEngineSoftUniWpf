using ChessEngine.Data.Common.Events;
using ChessEngine.Services.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Events
{
    public class SettingsService : IEventService<SettingsChangeEventArgs>
    {
        public event EventHandler<SettingsChangeEventArgs> StateChanged;

        public void ChangeState(SettingsChangeEventArgs args)
        {
            if (StateChanged != null)
            {
                StateChanged(this, args);
            }
        }
    }
}
