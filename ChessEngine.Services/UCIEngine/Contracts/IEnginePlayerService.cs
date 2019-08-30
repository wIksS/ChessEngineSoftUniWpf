using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.UCIEngine.Contracts
{
    public interface IEnginePlayerService
    {
        void InitPlayer(int waitingTimePerMove);

        Task<string> PlayMove(string move, bool autoplay = false);
    }
}
