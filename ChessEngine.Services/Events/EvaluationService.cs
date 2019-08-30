using ChessEngine.Data.Common.Events;
using ChessEngine.Services.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Events
{
    public class EvaluationService : IEventService<EvaluationChangeEventArgs>
    {
        public event EventHandler<EvaluationChangeEventArgs> StateChanged;

        public void ChangeState(EvaluationChangeEventArgs args)
        {
            if (StateChanged != null)
            {
                StateChanged(this, args);
            }
        }
    }
}
