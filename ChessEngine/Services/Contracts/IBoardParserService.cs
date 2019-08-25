using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Contracts
{
	public interface IBoardParserService
	{
		string Generate_simple_fen_from_board(Square[,] board);
		string Generate_full_fen_from_board(Square[,] Board, bool WhiteToMove, bool WKingSideCastle, bool BKingSideCastle, bool WQueenSideCastle, bool BQueenSideCastle, int EnPasRow, int EnPasCol, int FiftyRuleCounter, int FullMove);
	}
}
