using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Contracts
{
    public interface IChessRulesService
    {
        bool Check(Square[,] board, Knight from, ChessFigure to);
        bool Check(Square[,] board, Pawn from, ChessFigure to);

        bool Check(Square[,] board, ChessFigure from, ChessFigure to);
    }
}
