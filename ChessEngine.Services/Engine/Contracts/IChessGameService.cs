using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Engine.Contracts
{
	public interface IChessGameService
	{
		bool Fifty_move_rule();
		bool Threefold_repetition();

		bool Process_move(Square[,] Board, ChessMoveInfo MoveInfo);
	}
}
