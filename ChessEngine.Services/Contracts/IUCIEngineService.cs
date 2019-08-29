using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Contracts
{
    public interface IUCIEngineService
    {
        void AddListener(UCIListener newListener);
        void AddMove(string move);
        void PlayMoveDepth(string move, int depth);
        void PlayMoveTime(string move, int mseconds);
        void EvalPositionDepth(int depth);
        void Init(int breakAfterMiliseconds);
    }
}
