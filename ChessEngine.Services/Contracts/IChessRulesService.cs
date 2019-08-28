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
		ChessMoveInfo Check(Square[,] board, Pawn from, ChessFigure to);
		ChessMoveInfo Check(Square[,] board, Rook from, ChessFigure to);
		ChessMoveInfo Check(Square[,] board, Knight from, ChessFigure to);
		ChessMoveInfo Check(Square[,] board, Bishop from, ChessFigure to);
		ChessMoveInfo Check(Square[,] board, King from, ChessFigure to);
		ChessMoveInfo Check(Square[,] board, Queen from, ChessFigure to);

		ChessMoveInfo Check(Square[,] board, ChessFigure from, ChessFigure to);
    }
}
