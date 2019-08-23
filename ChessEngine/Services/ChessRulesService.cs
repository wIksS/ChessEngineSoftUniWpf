using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services
{
    public class ChessRulesService : IChessRulesService
    {
        public bool Check(Square[,] board, Knight from, ChessFigure to)
        {
            if (Math.Abs(from.Row - to.Row) == 2)
            {
                if (Math.Abs(from.Col - to.Col) == 1)
                {
                    return true;
                }
            }
            if (Math.Abs(from.Row - to.Row) == 1)
            {
                if (Math.Abs(from.Col - to.Col) == 2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Check(Square[,] board, Pawn from, ChessFigure to)
        {
            if (from.IsWhite)
            {
                if (from.Row - to.Row == 1)
                {
                    return true;
                }
            }
            else
            {
                if (to.Row - from.Row == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Check(Square[,] board, ChessFigure from, ChessFigure to)
        {
            return false;
        }
    }
}
