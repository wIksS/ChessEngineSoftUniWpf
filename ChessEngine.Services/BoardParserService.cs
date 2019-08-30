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
	public class BoardParserService : IBoardParserService
	{
		/// <summary>
		/// Creates only the board part of the fen
		/// </summary>
		/// <param name="Board"> The Board </param>
		/// <returns></returns>
		public string Generate_simple_fen_from_board(Square[,] Board)
		{
			Debug.Assert(Board.GetLength(0) == 8 && Board.GetLength(1) == 8, "Trying to Generate fen from a board which is not 8x8");
			StringBuilder fen = new StringBuilder();
			for (int i = 0; i < Board.GetLength(0); i++)
			{
				int emptyCounter = 0;
				for (int j = 0; j < Board.GetLength(1); j++)
				{
					switch (Board[i, j].Figure.Name)
					{
						case "Empty":
							emptyCounter++;
							break;
						case "Pawn":
							if (emptyCounter > 0)
							{
								fen.Append(((char)('0' + emptyCounter)).ToString());
								emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'P' : 'p')).ToString());
							break;
						case "Rook":
							if (emptyCounter > 0)
							{
                                fen.Append(((char)('0' + emptyCounter)).ToString());
                                emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'R' : 'r')).ToString());
							break;
						case "Knight":
							if (emptyCounter > 0)
							{
                                fen.Append(((char)('0' + emptyCounter)).ToString());
                                emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'N' : 'n')).ToString());
							break;
						case "Bishop":
							if (emptyCounter > 0)
							{
                                fen.Append(((char)('0' + emptyCounter)).ToString());
                                emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'B' : 'b')).ToString());
							break;
						case "King":
							if (emptyCounter > 0)
							{
                                fen.Append(((char)('0' + emptyCounter)).ToString());
                                emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'K' : 'k')).ToString());
							break;
						case "Queen":
							if (emptyCounter > 0)
							{
                                fen.Append(((char)('0' + emptyCounter)).ToString());
                                emptyCounter = 0;
							}
							fen.Append(((char)(Board[i, j].Figure.IsWhite ? 'Q' : 'q')).ToString());
							break;
					}
				}
				if (emptyCounter > 0)
				{
                    fen.Append(((char)('0' + emptyCounter)).ToString());
                    emptyCounter = 0;
				}
				if (i != Board.GetLength(0) - 1) fen.Append( "/" );
			}

			return fen.ToString();
		}

		public string Generate_full_fen_from_board(Square[,] Board, bool WhiteToMove, bool WKingSideCastle, bool BKingSideCastle, bool WQueenSideCastle, bool BQueenSideCastle, int EnPasRow, int EnPasCol, int FiftyRuleCounter, int FullMove)
		{
			Debug.Assert(Board.GetLength(0) == 8 && Board.GetLength(1) == 8, "Trying to Generate fen from a board which is not 8x8");
			StringBuilder fen = new StringBuilder(Generate_simple_fen_from_board(Board));
			fen.Append(WhiteToMove ? " w " : " b ");

			if (WKingSideCastle)  fen.Append("K");
			if (WQueenSideCastle) fen.Append("Q");
			if (BKingSideCastle)  fen.Append("k");
			if (BQueenSideCastle) fen.Append("q");

			if (!WKingSideCastle && !WQueenSideCastle && !BKingSideCastle && !BQueenSideCastle)
			{
				fen.Append("- ");
			}
			else fen.Append(" ");

			if(EnPasRow == Constants.OffBoard && EnPasCol == Constants.OffBoard)
			{
				fen.Append("- ");
			}
			else
			{
				fen.Append(Square_parser(EnPasRow, EnPasCol));
				fen.Append(" ");
			}

			fen.Append(FiftyRuleCounter.ToString());
			fen.Append(" ");
			fen.Append(FullMove.ToString());

			return fen.ToString();
		}

        public string Square_parser(int row, int col)
        {
            row = 8 - row;
            string square = "";
            square += ((char)('a' + col)).ToString();
            square += ((char)('0' + row)).ToString();
            return square;
        }

        public string MoveParserUCI(ChessMoveInfo moveInfo)
        {
            string move = Square_parser(moveInfo.FromRow, moveInfo.FromCol) + Square_parser(moveInfo.ToRow, moveInfo.ToCol);
            if (moveInfo.IsPromotion) move += "q";
            return move;
        }
    }
}
