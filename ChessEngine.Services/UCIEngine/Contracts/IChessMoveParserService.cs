using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.UCIEngine.Contracts
{
    public interface IChessMoveParserService
    {
        Tuple<int, int, int, int> ParseString(string move);

        string CastPosition(int fromRow, int fromCol, int toRow, int toCol);
    }
}
