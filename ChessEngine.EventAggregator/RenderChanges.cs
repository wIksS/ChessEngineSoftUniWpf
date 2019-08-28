using ChessEngine.Data;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.EventAggregatorNamespace
{
    public class RenderChanges : PubSubEvent<RenderSettings>
    {
    }
}
