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
		public bool IsAllowed { get; set; }
		public bool MovedFigureIsWhite { get; set; }
		public bool EnemyKingIsCheck { get; set; }
		public bool IsPromotion { get; set; }
		public bool WasEnPas { get; set; }
		public int EnPasRow { get; set; }
		public int EnPasCol { get; set; }

		public ChessMoveInfo(bool IsAllowed = false)
		{
			this.IsAllowed = IsAllowed;
			MovedFigureIsWhite = false;
			EnemyKingIsCheck = false;
			IsPromotion = false;
			EnPasRow = EnPasCol = Constants.OffBoard;
			WasEnPas = false;
		}

		public ChessMoveInfo(bool IsAllowed = false, bool MovedFigureIsWhite = false)
		{
			this.IsAllowed = IsAllowed;
			this.MovedFigureIsWhite = MovedFigureIsWhite;
			EnemyKingIsCheck = false;
			IsPromotion = false;
			EnPasRow = EnPasCol = Constants.OffBoard;
			WasEnPas = false;
		}

		public ChessMoveInfo(bool IsAllowed = false, bool MovedFigureIsWhite = false, bool EnemyKingIsCheck = false,
			bool IsPromotion = false, int EnPasRow = Constants.OffBoard, int EnPasCol = Constants.OffBoard, bool WasEnPas = false)
		{
			this.IsAllowed = IsAllowed;
			this.MovedFigureIsWhite = MovedFigureIsWhite;
			this.EnemyKingIsCheck = EnemyKingIsCheck;
			this.IsPromotion = IsPromotion;
			this.EnPasRow = EnPasRow;
			this.EnPasCol = EnPasCol;
			this.WasEnPas = WasEnPas;
		}
	}
}
