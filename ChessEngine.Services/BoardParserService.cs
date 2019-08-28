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
		/// NOT TESTED!!!
		/// </summary>
		/// <param name="Board"> The Board </param>
		/// <returns></returns>
		public string Generate_simple_fen_from_board(Square[,] Board)
		{
			Debug.Assert(Board.GetLength(0) == 8 && Board.GetLength(1) == 8, "Trying to Generate fen from a board which is not 8x8");
			string fen = "";
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
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'P' : 'p');
							break;
						case "Rook":
							if (emptyCounter > 0)
							{
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'R' : 'r');
							break;
						case "Knight":
							if (emptyCounter > 0)
							{
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'N' : 'n');
							break;
						case "Bishop":
							if (emptyCounter > 0)
							{
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'B' : 'b');
							break;
						case "King":
							if (emptyCounter > 0)
							{
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'K' : 'k');
							break;
						case "Queen":
							if (emptyCounter > 0)
							{
								fen += ('0' + emptyCounter);
								emptyCounter = 0;
							}
							fen += (Board[i, j].Figure.IsWhite ? 'Q' : 'q');
							break;
					}
				}
				if (emptyCounter > 0)
				{
					fen += ('0' + emptyCounter);
					emptyCounter = 0;
				}
				if (i != Board.GetLength(0) - 1) fen += "/";
			}

			return fen;
		}

		public string Square_translator(int row, int col)
		{
			row = 8 - row;
			string square = "";
			square += ('a' + col);
			square += ('0' + row);
			return square;
		}

		public string Generate_full_fen_from_board(Square[,] Board, bool WhiteToMove, bool WKingSideCastle, bool BKingSideCastle, bool WQueenSideCastle, bool BQueenSideCastle, int EnPasRow, int EnPasCol, int FiftyRuleCounter, int FullMove)
		{
			Debug.Assert(Board.GetLength(0) == 8 && Board.GetLength(1) == 8, "Trying to Generate fen from a board which is not 8x8");
			string fen = Generate_simple_fen_from_board(Board);
			fen += (WhiteToMove ? " w " : " b ");

			if (WKingSideCastle)  fen += "K";
			if (WQueenSideCastle) fen += "Q";
			if (BKingSideCastle)  fen += "k";
			if (BQueenSideCastle) fen += "q";

			if (!(WKingSideCastle | WQueenSideCastle | BKingSideCastle | BQueenSideCastle))
			{
				fen += "- ";
			}
			else fen += " ";

			if(EnPasRow == Constants.OffBoard && EnPasCol == Constants.OffBoard)
			{
				fen += "- ";
			}
			else
			{
				fen += Square_translator(EnPasRow, EnPasCol);
				fen += " ";
			}

			fen += FiftyRuleCounter.ToString();
			fen += " ";
			fen += FullMove.ToString();

			return fen;
		}
		
	}
}
