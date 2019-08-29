using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services
{
    public class ChessRulesService : IChessRulesService
    {
		private bool Check_for_color_match(ChessFigure A, ChessFigure B)
		{
			if (A.Name == "Empty" || B.Name == "Empty") return false;
			return A.IsWhite == B.IsWhite;
		}

		private bool Square_is_empty(Square Sq)
		{
			return Sq.Figure.Name == "Empty";
		}

		public bool Check_for_check(bool KingIsWhite, Square[,] Board)
		{
			bool[,] AttackBoard = Generate_attack_board(Board, !KingIsWhite); // Generate the attack board for the opposite color

			return Check_for_check(KingIsWhite, Board, AttackBoard);
		}

		public bool Check_for_check(bool KingIsWhite, Square[,] Board, bool[,] AttackBoard)
		{
			
			foreach (var sq in Board)
			{
				if (sq.Figure.Name == "King" && sq.Figure.IsWhite == KingIsWhite)
				{
					return AttackBoard[sq.Row, sq.Col];
				}
			}
			
			return false;
		}

		public bool[,] Generate_attack_board(Square[,] Board, bool IsWhite)
		{
			bool[,] AttackBoard = new bool[Constants.BoardRows, Constants.BoardCols];

			void ArrayAndOperation(bool[,] arr)
			{
				for(int i = 0; i < arr.GetLength(0); i++)
				{
					for(int j = 0; j < arr.GetLength(1); j++)
					{
						AttackBoard[i, j] |= arr[i, j]; 
					}
				}
			}

			foreach (var sq in Board)
				if (sq.Figure.IsWhite == IsWhite && sq.Figure.Name != "Empty")
					ArrayAndOperation(Generate_attack_board(Board, sq.Figure));

			return AttackBoard;
		}

		public bool[,] Generate_attack_board(Square[,] Board, ChessFigure fig)
		{
			Debug.Assert(fig.Name != "Empty", "Trying to generate attack field for a figure of type Empty");

			bool[,] AttackBoard = new bool[Constants.BoardRows,Constants.BoardCols];

			int Row = fig.Row;
			int Col = fig.Col;
			bool IsWhite = fig.IsWhite;
			string Name = fig.Name;

			bool ValidateAndChange(int i, int j, bool val)
			{
				if (i < 0 || i >= Constants.BoardRows) return false;
				if (j < 0 || j >= Constants.BoardCols) return false;
				AttackBoard[i, j] = val;
				return true;
			}

			void PawnAttack()
			{
				if (IsWhite)
				{
					ValidateAndChange(Row - 1, Col - 1, true);
					ValidateAndChange(Row - 1, Col + 1, true);
				}
				else
				{
					ValidateAndChange(Row + 1, Col - 1, true);
					ValidateAndChange(Row + 1, Col + 1, true);
				}
			}

			void KnightAttack()
			{
				ValidateAndChange(Row - 1, Col + 2, true);
				ValidateAndChange(Row + 1, Col + 2, true);
				ValidateAndChange(Row - 1, Col - 2, true);
				ValidateAndChange(Row + 1, Col - 2, true);

				ValidateAndChange(Row - 2, Col + 1, true);
				ValidateAndChange(Row + 2, Col + 1, true);
				ValidateAndChange(Row - 2, Col - 1, true);
				ValidateAndChange(Row + 2, Col - 1, true);
			}

			void KingAttack()
			{
				ValidateAndChange(Row - 1, Col + 1, true);
				ValidateAndChange(Row + 1, Col + 1, true);
				ValidateAndChange(Row - 1, Col - 1, true);
				ValidateAndChange(Row + 1, Col - 1, true);

				ValidateAndChange(Row - 1, Col, true);
				ValidateAndChange(Row + 1, Col, true);
				ValidateAndChange(Row, Col + 1, true);
				ValidateAndChange(Row, Col - 1, true);
			}

			void BishopAttack()
			{
				for(int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row - i, Col - i, true)) break;
					if (!Square_is_empty(Board[Row - i, Col - i])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row + i, Col + i, true)) break;
					if (!Square_is_empty(Board[Row + i, Col + i])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row - i, Col + i, true)) break;
					if (!Square_is_empty(Board[Row - i, Col + i])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row + i, Col - i, true)) break;
					if (!Square_is_empty(Board[Row + i, Col - i])) break;
				}
			}

			void RookAttack()
			{
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row - i, Col, true)) break;
					if (!Square_is_empty(Board[Row - i, Col])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row + i, Col, true)) break;
					if (!Square_is_empty(Board[Row + i, Col])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row, Col + i, true)) break;
					if (!Square_is_empty(Board[Row, Col + i])) break;
				}
				for (int i = 1; ; i++)
				{
					if (!ValidateAndChange(Row, Col - i, true)) break;
					if (!Square_is_empty(Board[Row, Col - i])) break;
				}
			}

			void QueenAttack()
			{
				RookAttack();
				BishopAttack();
			}

			if (Name == "Pawn")
			{
				PawnAttack();
			}else if(Name == "Knight")
			{
				KnightAttack();
			}
			else if(Name == "King")
			{
				KingAttack();
			}
			else if(Name == "Bishop")
			{
				BishopAttack();
			}
			else if(Name == "Rook")
			{
				RookAttack();
			}
			else if(Name == "Queen")
			{
				QueenAttack();
			}

			return AttackBoard;
		}

		public bool Check_if_king_is_check_after(Square[,] board, ChessFigure from, ChessFigure to)
		{
			int tempCol = from.Col;
			int tempRow = from.Row;

			board[from.Row, from.Col].Figure = new Empty(from.Row, from.Col);
			board[to.Row, to.Col].Figure = from;
			from.Row = to.Row;
			from.Col = to.Col;

			bool IsCheck = Check_for_check(from.IsWhite, board);

			from.Row = tempRow;
			from.Col = tempCol;
			board[tempRow, tempCol].Figure = from;
			board[to.Row, to.Col].Figure = to;

			return IsCheck;
		}

		public ChessMoveInfo Check(Square[,] board, Knight from, ChessFigure to)
        {
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);

			bool KingIsInDanger = Check_for_check(from.IsWhite, board); // If my King is in danger this move should save it;
			if (KingIsInDanger)
			{
				if (Check_if_king_is_check_after(board, from, to))
					// We don't care if the move is valid as long as it saves the king.
					// After that initial check, it is asserted whether the move is valid or not
					// and the necessary measures are taken.
					return new ChessMoveInfo(false);
			}
			if(Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			///Again we don't care if it's a valid move just yet,
			///just checking if the figure were to move to the designated
			///position will it endanger the enemy king or not.
			bool IsEnemyKingCheck = Check_for_check(!from.IsWhite, board, Generate_attack_board(board, new Knight(to.Row, to.Col, from.IsWhite, "")));

			ChessMoveInfo MoveInfo = new ChessMoveInfo(true, from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			if (Math.Abs(from.Row - to.Row) == 2)
            {
                if (Math.Abs(from.Col - to.Col) == 1)
                {
					return MoveInfo;
				}
            }
            if (Math.Abs(from.Row - to.Row) == 1)
            {
                if (Math.Abs(from.Col - to.Col) == 2)
                {

					return MoveInfo;
				}
            }
			return new ChessMoveInfo(false);
        }

        public ChessMoveInfo Check(Square[,] board, Pawn from, ChessFigure to)
        {
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);
			bool KingIsInDanger = Check_for_check(from.IsWhite, board); // If my King is in danger this move should save it;

			if (KingIsInDanger)
			{
				if(Check_if_king_is_check_after(board, from, to))
					// We don't care if the move is valid as long as it saves the king.
					// After that initial check, it is asserted whether the move is valid or not
					// and the necessary measures are taken.
					return new ChessMoveInfo(false);
			}
			if (Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			bool IsPromotion;

			///Again we don't care if it's a valid move just yet,
			///just checking if the figure were to move to the designated
			///position will it endanger the enemy king or not.
			bool IsEnemyKingCheck = Check_for_check(!from.IsWhite, board, Generate_attack_board(board, new Pawn(to.Row, to.Col, from.IsWhite, "")));

			ChessMoveInfo MoveInfo = new ChessMoveInfo(false,from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			if (from.IsWhite)
            {
				IsPromotion = (to.Row == 0);
				if (from.Row - to.Row == 1)
				{
					if (to.Col == from.Col && Square_is_empty(board[to.Row, to.Col]))
					{
						MoveInfo.IsAllowed = true;
						MoveInfo.IsPromotion = IsPromotion;
						MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
						return MoveInfo;
					}
					else if (Math.Abs(to.Col - from.Col) == 1)
					{
						// If there is an enemy there then return true.
						if (!Square_is_empty(board[to.Row, to.Col]) && to.IsWhite != from.IsWhite)
						{
							MoveInfo.IsAllowed = true;
							MoveInfo.IsPromotion = IsPromotion;
							MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
							return MoveInfo;
						}
						else if (board[to.Row, to.Col].EnPasPossible && board[to.Row, to.Col].EnPasIsWhite == from.IsWhite)
						{
							MoveInfo.IsAllowed = true;
							MoveInfo.IsPromotion = IsPromotion;
							MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
							MoveInfo.TakenFigureRow = to.Row + 1;
							MoveInfo.TakenFigureCol = to.Col;
							return MoveInfo;
						}
					}
				}
				else
				// To make the board scalable we use Constants and not rows 6 and 4
				if (from.Row == Constants.BoardRows-2 && to.Row == Constants.BoardRows - 4 && from.Col == to.Col &&
					Square_is_empty(board[Constants.BoardRows - 4, from.Col]))
				{
					//Check if the square between the destination and the figure is empty;
					if (Square_is_empty(board[from.Row - 1, from.Col]))
					{
						MoveInfo.IsAllowed = true;
						MoveInfo.IsPromotion = IsPromotion;
						MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
						MoveInfo.EnPasRow = from.Row - 1;
						MoveInfo.EnPasCol = from.Col;

						return MoveInfo; // There is a possible En Passant
					}
				}
			}
            else
            {
				IsPromotion = (to.Row == Constants.BoardRows-1);
				if (to.Row - from.Row == 1)
				{
					if (to.Col == from.Col && Square_is_empty(board[to.Row, to.Col]))
					{
						MoveInfo.IsAllowed = true;
						MoveInfo.IsPromotion = IsPromotion;
						MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
						return MoveInfo;
					}
					else if (Math.Abs(to.Col - from.Col) == 1)
					{
						// If there is an enemy there then return true.
						if (!Square_is_empty(board[to.Row, to.Col]) && to.IsWhite != from.IsWhite)
						{
							MoveInfo.IsAllowed = true;
							MoveInfo.IsPromotion = IsPromotion;
							MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
							return MoveInfo;
						}
						//Check if the to square is En Passant Square
						else if (board[to.Row, to.Col].EnPasPossible && board[to.Row, to.Col].EnPasIsWhite == from.IsWhite)
						{
							MoveInfo.IsAllowed = true;
							MoveInfo.IsPromotion = IsPromotion;
							MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
							MoveInfo.TakenFigureRow = to.Row - 1;
							MoveInfo.TakenFigureCol = to.Col;
							return MoveInfo;
						}
					}
				}
				else
				if (from.Row == 1 && to.Row == 3 && from.Col == to.Col &&
					Square_is_empty(board[3, from.Col]))
				{
					//Check if the square between the destination and the figure is empty;
					if (Square_is_empty(board[from.Row + 1, from.Col]))
					{
						MoveInfo.IsAllowed = true;
						MoveInfo.IsPromotion = IsPromotion;
						MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
						MoveInfo.EnPasRow = from.Row + 1;
						MoveInfo.EnPasCol = from.Col;
						return MoveInfo; // There is a possible En Passant
					}
				}
			}
            return new ChessMoveInfo(false);
        }

		public ChessMoveInfo Check(Square[,] board, Rook from, ChessFigure to)
		{
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);
			bool KingIsInDanger = Check_for_check(from.IsWhite, board); // If my King is in danger this move should save it;

			if (KingIsInDanger)
			{
				if (Check_if_king_is_check_after(board, from, to))
					// We don't care if the move is valid as long as it saves the king.
					// After that initial check, it is asserted whether the move is valid or not
					// and the necessary measures are taken.
					return new ChessMoveInfo(false);
			}
			if (Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			///Again we don't care if it's a valid move just yet,
			///just checking if the figure were to move to the designated
			///position will it endanger the enemy king or not.
			bool IsEnemyKingCheck = Check_for_check(!from.IsWhite, board, Generate_attack_board(board,new Rook(to.Row,to.Col,from.IsWhite,"")));

			ChessMoveInfo MoveInfo = new ChessMoveInfo(false, from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			if (from.Row == to.Row)
			{
				for (int i = from.Col; i != to.Col; i += (from.Col > to.Col ? -1 : 1)) 
				{
					if (i == from.Col) continue;
					if (!Square_is_empty(board[from.Row, i])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			else if (from.Col == to.Col)
			{
				for (int i = from.Row; i != to.Row; i += (from.Row > to.Row ? -1 : 1))
				{
					if (i == from.Row) continue;
					if (!Square_is_empty(board[i, from.Col])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}else return new ChessMoveInfo(false);
		}

		public ChessMoveInfo Check(Square[,] board, Bishop from, ChessFigure to)
		{
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);
			bool KingIsInDanger = Check_for_check(from.IsWhite, board); // If my King is in danger this move should save it;

			if (KingIsInDanger)
			{
				if (Check_if_king_is_check_after(board, from, to))
					// We don't care if the move is valid as long as it saves the king.
					// After that initial check, it is asserted whether the move is valid or not
					// and the necessary measures are taken.
					return new ChessMoveInfo(false);
			}
			if (Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			///Again we don't care if it's a valid move just yet,
			///just checking if the figure were to move to the designated
			///position will it endanger the enemy king or not.
			bool IsEnemyKingCheck = Check_for_check(!from.IsWhite, board, Generate_attack_board(board, new Bishop(to.Row, to.Col, from.IsWhite, "")));

			ChessMoveInfo MoveInfo = new ChessMoveInfo(false, from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			int deltaRow = from.Row - to.Row;
			int deltaCol = from.Col - to.Col;
			if (Math.Abs(deltaRow) != Math.Abs(deltaCol)) return new ChessMoveInfo(false);
			if (deltaRow > 0 && deltaCol > 0) // Up Left
			{
				for(int i = from.Row-1, j = from.Col-1; i != to.Row; i--, j--)
				{
					if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			if (deltaRow < 0 && deltaCol < 0) // Down Right
			{
				for (int i = from.Row + 1, j = from.Col + 1; i != to.Row; i++, j++)
				{
					if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			if (deltaRow > 0 && deltaCol < 0) // Up Right
			{
				for (int i = from.Row - 1, j = from.Col + 1; i != to.Row; i--, j++)
				{
					if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			if (deltaRow < 0 && deltaCol > 0) // Down Left
			{
				for (int i = from.Row + 1, j = from.Col - 1; i != to.Row; i++, j--)
				{
					if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			return new ChessMoveInfo(false);
		}

		public ChessMoveInfo Check(Square[,] board, King from, ChessFigure to)
		{
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);
			
			if (Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			ChessMoveInfo MoveInfo = new ChessMoveInfo(true, from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			if (Math.Abs(from.Col - to.Col) == 1 && Math.Abs(from.Row - to.Row) == 1)
			{
				return MoveInfo;
			}
			if (Math.Abs(from.Col - to.Col) == 1 && Math.Abs(from.Row - to.Row) == 0)
			{
				return MoveInfo;
			}
			if (Math.Abs(from.Col - to.Col) == 0 && Math.Abs(from.Row - to.Row) == 1)
			{
				return MoveInfo;
			}
			if (Constants.BoardCols == 8 && Constants.BoardRows == 8 &&
				from.HasMoved == false) {
				if (from.IsWhite)
				{
					if ( to.Row == 7 && to.Col == 6 &&
						board[7, 7].Figure.Name=="Rook" && (board[7,7].Figure as Rook).HasMoved == false &&
						Square_is_empty(board[7,6]) && Square_is_empty(board[7, 5]))
					{
                        var atkBoard = Generate_attack_board(board, !from.IsWhite);
                        if (!atkBoard[7, 6] && !atkBoard[7, 5])
                        {
                            MoveInfo.WasKingSideCastle = true;
                            return MoveInfo;
                        }
					}
					if (to.Row == 7 && to.Col == 2 &&
						board[7, 0].Figure.Name == "Rook" && (board[7, 0].Figure as Rook).HasMoved == false &&
						Square_is_empty(board[7, 1]) && Square_is_empty(board[7, 2]) && Square_is_empty(board[7, 3]))
					{
                        var atkBoard = Generate_attack_board(board, !from.IsWhite);
                        if (!atkBoard[7, 1] && !atkBoard[7, 2] && !atkBoard[7, 3])
                        {
                            MoveInfo.WasQueenSideCastle = true;
                            return MoveInfo;
                        }
					}
				}
				else
				{
					if (to.Row == 0 && to.Col == 6 &&
						board[0, 7].Figure.Name == "Rook" && (board[0, 7].Figure as Rook).HasMoved == false &&
						Square_is_empty(board[0, 6]) && Square_is_empty(board[0, 5]))
					{
                        var atkBoard = Generate_attack_board(board, !from.IsWhite);
                        if (!atkBoard[0, 6] && !atkBoard[0, 5])
                        {
                            MoveInfo.WasKingSideCastle = true;
                            return MoveInfo;
                        }

					}
					if (to.Row == 0 && to.Col == 2 &&
						board[0, 0].Figure.Name == "Rook" && (board[0, 0].Figure as Rook).HasMoved == false &&
						Square_is_empty(board[0, 1]) && Square_is_empty(board[0, 2]) && Square_is_empty(board[0, 3]))
					{
                        var atkBoard = Generate_attack_board(board, !from.IsWhite);
                        if (!atkBoard[0, 1] && !atkBoard[0, 2] && !atkBoard[0, 3])
                        {
                            MoveInfo.WasQueenSideCastle = true;
                            return MoveInfo;
                        }
					}
				}
			}
			return new ChessMoveInfo(false);
		}

		public ChessMoveInfo Check(Square[,] board, Queen from, ChessFigure to)
		{
			if (Check_for_color_match(from, to)) return new ChessMoveInfo(false);
			bool KingIsInDanger = Check_for_check(from.IsWhite, board); // If my King is in danger this move should save it;

			if (KingIsInDanger)
			{
				if (Check_if_king_is_check_after(board, from, to))
					// We don't care if the move is valid as long as it saves the king.
					// After that initial check, it is asserted whether the move is valid or not
					// and the necessary measures are taken.
					return new ChessMoveInfo(false);
			}
			if (Check_if_king_is_check_after(board, from, to))
			{
				//Same idea but this time we check if the move puts
				//the life of our dear king in danger.
				return new ChessMoveInfo(false);
			}

			///Again we don't care if it's a valid move just yet,
			///just checking if the figure were to move to the designated
			///position will it endanger the enemy king or not.
			bool IsEnemyKingCheck = Check_for_check(!from.IsWhite, board, Generate_attack_board(board, new Queen(to.Row, to.Col, from.IsWhite, "")));

			ChessMoveInfo MoveInfo = new ChessMoveInfo(false, from.IsWhite);
			MoveInfo.FromRow = from.Row;
			MoveInfo.FromCol = from.Col;
			MoveInfo.ToRow = to.Row;
			MoveInfo.ToCol = to.Col;

			if (to.Name != "Empty")
			{
				MoveInfo.TakenFigureRow = to.Row;
				MoveInfo.TakenFigureCol = to.Col;
			}

			int deltaRow = from.Row - to.Row;
			int deltaCol = from.Col - to.Col;
			if (Math.Abs(deltaRow) == Math.Abs(deltaCol))
			{
				if (deltaRow > 0 && deltaCol > 0) // Up Left
				{
					for (int i = from.Row - 1, j = from.Col - 1; i != to.Row; i--, j--)
					{
						if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
					}
					MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
					MoveInfo.IsAllowed = true;
					return MoveInfo;
				}
				if (deltaRow < 0 && deltaCol < 0) // Down Right
				{
					for (int i = from.Row + 1, j = from.Col + 1; i != to.Row; i++, j++)
					{
						if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
					}
					MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
					MoveInfo.IsAllowed = true;
					return MoveInfo;
				}
				if (deltaRow > 0 && deltaCol < 0) // Up Right
				{
					for (int i = from.Row - 1, j = from.Col + 1; i != to.Row; i--, j++)
					{
						if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
					}
					MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
					MoveInfo.IsAllowed = true;
					return MoveInfo;
				}
				if (deltaRow < 0 && deltaCol > 0) // Down Left
				{
					for (int i = from.Row + 1, j = from.Col - 1; i != to.Row; i++, j--)
					{
						if (!Square_is_empty(board[i, j])) return new ChessMoveInfo(false);
					}
					MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
					MoveInfo.IsAllowed = true;
					return MoveInfo;
				}
				return new ChessMoveInfo(false);
			}
			else if (from.Row == to.Row)
			{
				for (int i = from.Col; i != to.Col; i += (from.Col > to.Col ? -1 : 1))
				{
					if (i == from.Col) continue;
					if (!Square_is_empty(board[from.Row, i])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			else if (from.Col == to.Col)
			{
				for (int i = from.Row; i != to.Row; i += (from.Row > to.Row ? -1 : 1))
				{
					if (i == from.Row) continue;
					if (!Square_is_empty(board[i, from.Col])) return new ChessMoveInfo(false);
				}
				MoveInfo.EnemyKingIsCheck = IsEnemyKingCheck;
				MoveInfo.IsAllowed = true;
				return MoveInfo;
			}
			else return new ChessMoveInfo(false);
		}

		public ChessMoveInfo Check(Square[,] board, ChessFigure from, ChessFigure to)
		{
			return new ChessMoveInfo(false);
		}
	}
}
