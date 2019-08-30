using ChessEngine.Data.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Events.Contracts
{
    public interface IEventService<T> where T : EventArgs
    {
        event EventHandler<T> StateChanged;

        void ChangeState(T args);
    }
}
