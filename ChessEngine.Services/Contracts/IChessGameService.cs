using ChessEngine.Common;
using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Contracts
{
	public interface IChessGameService
	{
		ChessMoveInfo Check(Square[,] board, ChessFigure from, ChessFigure to);

		EndCondition Check_for_end_condition(Square[,] board, bool IsWhite);

		List<ChessMoveInfo> Get_all_possible_moves(Square[,] Board, bool IsWhite);

		bool Process_move(Square[,] Board, ChessMoveInfo MoveInfo);
        
		GameSettings GetGameSetting();

		bool White_to_move();
	}
}
