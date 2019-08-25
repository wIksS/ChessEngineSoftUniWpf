#define QUICKFEN_THREEFOLD

using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessEngine.Services
{
	public class ChessGameService : IChessGameService
	{
		GameMode MODE;
		ChessGameInfo GameInfo;
		ChessRulesService RuleService { get; set; }
		IBoardParserService BoardParser { get; set; }
		int FiftyMoveCounter { get; set; }
		Dictionary<string, int> FenPositionCounter { get; set; }

		public ChessGameService()
		{
			MODE |= GameMode.WHITEBLACK;

			GameInfo = new ChessGameInfo();
			FiftyMoveCounter = 0;
			FenPositionCounter = new Dictionary<string, int>();
			RuleService = new ChessRulesService();
			BoardParser = new BoardParserService();
		}

		//Returns whether the position is inside of the board
		private bool Validate_position(int i, int j)
		{
			if (i < 0 || i >= Constants.BoardRows) return false;
			if (j < 0 || j >= Constants.BoardCols) return false;
			return true;
		}

		private List<Tuple<int,int>> Get_move_pattern(string Type, int Row, int Col, bool IsWhite)
		{
			List<Tuple<int, int>> MovePattern = new List<Tuple<int, int>>();

			void AddPattern(int i, int j)
			{
				if (Validate_position(i, j))
				{
					MovePattern.Add(new Tuple<int, int>(i, j));
				}
			}

			if(Type == "Pawn")
			{
				if (IsWhite)
				{
					AddPattern(Row - 1, Col);
					AddPattern(Row - 1, Col-1);
					AddPattern(Row - 1, Col+1);
					if (Row == 6)
					{
						AddPattern(Row - 2, Col);
					}
				}
				else
				{
					AddPattern(Row + 1, Col);
					AddPattern(Row + 1, Col - 1);
					AddPattern(Row + 1, Col + 1);
					if (Row == 1)
					{
						AddPattern(Row + 2, Col);
					}
				}
			}
			if(Type == "Rook" || Type == "Queen")
			{
				for(int i = 0; i < Constants.BoardRows; i++)
				{
					AddPattern(Row - i, Col);
					AddPattern(Row + i, Col);
				}
				for (int i = 0; i < Constants.BoardCols; i++)
				{
					AddPattern(Row, Col - i);
					AddPattern(Row, Col + i);
				}
			}
			if (Type == "Bishop" || Type == "Queen")
			{
				for (int i = 0; i < Constants.BoardRows; i++)
				{
					AddPattern(Row - i, Col - i);
					AddPattern(Row - i, Col + i);
					AddPattern(Row + i, Col - i);
					AddPattern(Row + i, Col + i);
				}
			}
			if (Type == "Knight")
			{
				AddPattern(Row - 2, Col - 1);
				AddPattern(Row - 2, Col + 1);
				AddPattern(Row + 2, Col - 1);
				AddPattern(Row + 2, Col + 1);

				AddPattern(Row - 1, Col - 2);
				AddPattern(Row - 1, Col + 2);
				AddPattern(Row + 1, Col - 2);
				AddPattern(Row + 1, Col + 2);
			}
			if (Type == "King")
			{
				AddPattern(Row, Col - 1);
				AddPattern(Row, Col + 1);
				AddPattern(Row - 1, Col);
				AddPattern(Row + 1, Col);
								 
				AddPattern(Row - 1, Col - 1);
				AddPattern(Row - 1, Col + 1);
				AddPattern(Row + 1, Col - 1);
				AddPattern(Row + 1, Col + 1);
			}
			return MovePattern;
		}

		public List<ChessMoveInfo> Get_all_possible_moves(Square[,] Board, bool IsWhite)
		{
			List<ChessMoveInfo> PossibleMoves = new List<ChessMoveInfo>();

			foreach(Square sq in Board)
			{
				if(sq.Figure.IsWhite == IsWhite)
				{
					dynamic myFig = sq.Figure;
					List<Tuple<int, int>> MovePattern = Get_move_pattern(sq.Figure.Name, sq.Row, sq.Col, IsWhite);
					foreach(var pattern in MovePattern) {
						ChessFigure enemyFig = Board[pattern.Item1, pattern.Item2].Figure;
						ChessMoveInfo MoveInfo = RuleService.Check(Board, myFig, enemyFig);
						if (MoveInfo)
						{
							PossibleMoves.Add(MoveInfo);
						}
					}
				}
			}

			return PossibleMoves;
		}

		public bool Is_stalemate(Square[,] Board, bool IsWhite)
		{
			List<ChessMoveInfo> PossibleMoves = Get_all_possible_moves(Board, IsWhite);
			return PossibleMoves.Count == 0;
		}

		public bool Is_checkmate(Square[,] Board, bool IsWhite)
		{
			bool IsStalemate = Is_stalemate(Board, IsWhite);
			bool IsCheck = RuleService.Check_for_check(IsWhite, Board);
			return IsStalemate & IsCheck;
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

		public EndCondition Check_for_end_condition(Square[,] Board, bool IsWhite)
		{
			if (Is_checkmate(Board, IsWhite))
			{
				MessageBox.Show("Game ended by checkmate.");
				return EndCondition.Checkmate;
			}else if (Is_stalemate(Board, IsWhite))
			{
				MessageBox.Show("Game ended by stalemate.");
				return EndCondition.Stalemate;
			}else if (Fifty_move_rule())
			{
				MessageBox.Show("Game ended by Fifty move rule.");
				return EndCondition.FiftyRule;
			}else if (Threefold_repetition())
			{
				MessageBox.Show("Game ended by Threefold repetition.");
				return EndCondition.Threefold;
			}
			return EndCondition.None;
		}

		public ChessMoveInfo Check(Square[,] board, ChessFigure from, ChessFigure to)
		{
			if((MODE & GameMode.WHITEBLACK) == GameMode.WHITEBLACK)
				if (from.IsWhite != GameInfo.WhiteToMove) return new ChessMoveInfo(false);

			dynamic dFrom = from;
			dynamic dTo = to;
			return RuleService.Check(board, dFrom, dTo);
		}

		public bool Process_move(Square[,] Board, ChessMoveInfo MoveInfo)
		{
			if (!MoveInfo.IsAllowed) return false;

			GameInfo.WhiteToMove = !GameInfo.WhiteToMove;
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

#if QUICKFEN_THREEFOLD
			string currFen = BoardParser.Generate_simple_fen_from_board(Board);
			if (FenPositionCounter.ContainsKey(currFen)) FenPositionCounter[currFen] += 1;
			else FenPositionCounter.Add(currFen, 1);
#endif
			return true;
		}

		public bool White_to_move()
		{
			return GameInfo.WhiteToMove;
		}
	}
}
