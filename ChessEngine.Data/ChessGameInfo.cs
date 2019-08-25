using ChessEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
	public class ChessGameInfo
	{
		public bool WhiteToMove { get; set; }
		public bool WKingSideCastle { get; set; }
		public bool BKingSideCastle { get; set; }
		public bool WQueenSideCastle { get; set; }
		public bool BQueenSideCastle { get; set; }
		public int EnPasRow { get; set; }
		public int EnPasCol { get; set; }
		public int FiftyRuleCounter { get; set; }
		public int FullMoveCounter { get; set; }

		public List<ChessMoveInfo> History { get; set; }

		public ChessGameInfo()
		{
			List<ChessMoveInfo> History = new List<ChessMoveInfo>();
			WhiteToMove = true;
			WKingSideCastle = true;
			BKingSideCastle = true;
			WQueenSideCastle = true;
			BQueenSideCastle = true;
			EnPasRow = Constants.OffBoard;
			EnPasCol = Constants.OffBoard;
			FiftyRuleCounter = 0;
			FullMoveCounter = 0;
		}
	}
}
