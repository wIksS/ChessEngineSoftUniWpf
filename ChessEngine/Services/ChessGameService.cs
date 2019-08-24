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
	public class ChessGameService : IChessGameService
	{
		int FiftyMoveCounter { get; set; }
		Dictionary<string, int> FenPositionCounter { get; set; }

		public ChessGameService()
		{
			FiftyMoveCounter = 0;
			FenPositionCounter = new Dictionary<string, int>();
		}

		public bool Fifty_move_rule()
		{
			return FiftyMoveCounter >= 50;
		}
		
		public bool Threefold_repetition()
		{
			foreach (var pos in FenPositionCounter) if (pos.Value >= 3) return true;
			return false;
		}

		public bool Process_move(Square[,] Board, ChessMoveInfo MoveInfo)
		{
			if (!MoveInfo.IsAllowed) return false;

			FiftyMoveCounter++;

			Square from = Board[MoveInfo.FromRow, MoveInfo.FromCol];
			Square to = Board[MoveInfo.ToRow, MoveInfo.ToCol];

			// Check if pawn was moved
			if (from.Figure.Name == "Pawn")
				FiftyMoveCounter = 0;

			// Change to.figure to from.figure
			to.Figure = from.Figure;
			to.Figure.Col = to.Col;
			to.Figure.Row = to.Row;

			if (to.Figure.Name == "King")
				(to.Figure as King).HasMoved = true;
			if (to.Figure.Name == "Rook")
				(to.Figure as Rook).HasMoved = true;

			// Change from to empty
			from.Figure = new Empty(from.Row, from.Col);

			//Check if a capture was made and if move was en passant
			if (MoveInfo.TakenFigureCol != Constants.OffBoard && MoveInfo.TakenFigureRow != Constants.OffBoard)
			{
				FiftyMoveCounter = 0;
				if (MoveInfo.TakenFigureCol != MoveInfo.ToCol || MoveInfo.TakenFigureRow != MoveInfo.ToRow)
				{
					Board[MoveInfo.TakenFigureRow, MoveInfo.TakenFigureCol].Figure =
						new Empty(MoveInfo.TakenFigureRow, MoveInfo.TakenFigureCol);
				}
			}

			//Check if move was Castle
			if (MoveInfo.WasKingSideCastle)
			{
				if (to.Figure.IsWhite) // Move rook to 7,5
				{
					Board[7, 5].Figure = Board[7, 7].Figure;
					Board[7, 5].Figure.Row = 7;
					Board[7, 5].Figure.Col = 5;

					Board[7, 7].Figure = new Empty(7, 7);
				}
				if (!to.Figure.IsWhite) // Move rook to 0,5
				{
					Board[0, 5].Figure = Board[0, 7].Figure;
					Board[0, 5].Figure.Row = 0;
					Board[0, 5].Figure.Col = 5;

					Board[0, 7].Figure = new Empty(0, 7);
				}
				
			}
			if (MoveInfo.WasQueenSideCastle)
			{
				if (to.Figure.IsWhite)
				{
					Board[7, 3].Figure = Board[7, 0].Figure;
					Board[7, 3].Figure.Row = 7;
					Board[7, 3].Figure.Col = 3;

					Board[7, 0].Figure = new Empty(7, 0);
				}
				else
				{
					Board[0, 3].Figure = Board[0, 0].Figure;
					Board[0, 3].Figure.Row = 0;
					Board[0, 3].Figure.Col = 3;

					Board[0, 0].Figure = new Empty(0, 0);
				}
			}

			//Check if move was Pawn 2MoveAhead
			if (MoveInfo.EnPasRow != Constants.OffBoard && MoveInfo.EnPasCol != Constants.OffBoard)
			{
				Board[MoveInfo.EnPasRow, MoveInfo.EnPasCol].EnPasPossible = true;
				Board[MoveInfo.EnPasRow, MoveInfo.EnPasCol].EnPasIsWhite = !to.Figure.IsWhite;
			}

			return true;
		}
	}
}
