using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.Common;
namespace ChessEngine.Data
{
	public class ChessMoveInfo
	{
		public int FromRow { get; set; }
		public int FromCol { get; set; }
		public int ToRow { get; set; }
		public int ToCol { get; set; }

		public bool IsAllowed { get; set; }
		public bool MovedFigureIsWhite { get; set; }
		public bool EnemyKingIsCheck { get; set; }
		public bool IsPromotion { get; set; }
		public bool WasKingSideCastle { get; set; }
		public bool WasQueenSideCastle { get; set; }
		public int TakenFigureRow { get; set; }
		public int TakenFigureCol { get; set; }
		public int EnPasRow { get; set; }
		public int EnPasCol { get; set; }

		public ChessMoveInfo(bool IsAllowed = false)
		{
			this.IsAllowed = IsAllowed;
			MovedFigureIsWhite = false;
			EnemyKingIsCheck = false;
			IsPromotion = false;
			EnPasRow = EnPasCol = Constants.OffBoard;
			TakenFigureRow = Constants.OffBoard;
			TakenFigureCol = Constants.OffBoard;
			WasKingSideCastle = false;
			WasQueenSideCastle = false;
			FromRow = Constants.OffBoard;
			FromCol = Constants.OffBoard;
			ToRow = Constants.OffBoard;
			ToCol = Constants.OffBoard;
		}

		public ChessMoveInfo(bool IsAllowed = false, bool MovedFigureIsWhite = false)
		{
			this.IsAllowed = IsAllowed;
			this.MovedFigureIsWhite = MovedFigureIsWhite;
			EnemyKingIsCheck = false;
			IsPromotion = false;
			EnPasRow = EnPasCol = Constants.OffBoard;
			TakenFigureRow = Constants.OffBoard;
			TakenFigureCol = Constants.OffBoard;
			WasKingSideCastle = false;
			WasQueenSideCastle = false;
			FromRow = Constants.OffBoard;
			FromCol = Constants.OffBoard;
			ToRow = Constants.OffBoard;
			ToCol = Constants.OffBoard;
		}

		public ChessMoveInfo(bool IsAllowed = false, bool MovedFigureIsWhite = false, int TakenFigureRow = Constants.OffBoard, int TakenFigureCol = Constants.OffBoard)
		{
			this.IsAllowed = IsAllowed;
			this.MovedFigureIsWhite = MovedFigureIsWhite;
			EnemyKingIsCheck = false;
			IsPromotion = false;
			EnPasRow = EnPasCol = Constants.OffBoard;
			this.TakenFigureRow = TakenFigureRow;
			this.TakenFigureCol = TakenFigureCol;
			WasKingSideCastle = false;
			WasQueenSideCastle = false;
			FromRow = Constants.OffBoard;
			FromCol = Constants.OffBoard;
			ToRow = Constants.OffBoard;
			ToCol = Constants.OffBoard;
		}

		public ChessMoveInfo(bool IsAllowed = false, bool MovedFigureIsWhite = false, bool EnemyKingIsCheck = false,
			int TakenFigureRow = Constants.OffBoard, int TakenFigureCol = Constants.OffBoard,
			bool IsPromotion = false, int EnPasRow = Constants.OffBoard, int EnPasCol = Constants.OffBoard,
			bool WasKingSideCastle = false, bool WasQueenSideCastle = false,
			int FromRow = Constants.OffBoard, int FromCol = Constants.OffBoard, int ToRow = Constants.OffBoard, int ToCol = Constants.OffBoard)
		{
			this.IsAllowed = IsAllowed;
			this.MovedFigureIsWhite = MovedFigureIsWhite;
			this.EnemyKingIsCheck = EnemyKingIsCheck;
			this.TakenFigureRow = TakenFigureRow;
			this.TakenFigureCol = TakenFigureCol;
			this.IsPromotion = IsPromotion;
			this.EnPasRow = EnPasRow;
			this.EnPasCol = EnPasCol;
			this.WasKingSideCastle = WasKingSideCastle;
			this.WasQueenSideCastle = WasQueenSideCastle;
			this.FromRow = FromRow;
			this.FromCol = FromCol;
			this.ToRow = ToRow;
			this.ToCol = ToCol;
		}
	}
}
