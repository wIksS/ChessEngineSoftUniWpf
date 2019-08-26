using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessEngine.Services
{
    public class BoardGeneratorService : IBoardGeneratorService
    {
		/// <summary>
		/// Generates the board using a fen string
		/// For example this is the starting position
		/// rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR
		/// Lower case = black
		/// Upper case = WHITE
		/// </summary>
		/// <param name="fen">The fen string</param>
		/// <returns></returns>
		public Square[,] Generate_from_fen(string fen)
		{
			Debug.Assert(Constants.BoardRows == 8 && Constants.BoardCols == 8, "Trying to generate a board from a fen when the board is not 8x8");

			Square[,] Board = new Square[Constants.BoardRows, Constants.BoardCols];

			string[] tokens = fen.Split();

			int i = 0;
			int j = 0;
			foreach(char c in tokens[0])
			{
				if (c == '/')
				{
					i++;
					j = 0;
					continue;
				}

				switch (c)
				{
					case 'p':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Pawn(i, j, false, "/Images/blackpawn.png"));
						j++;
						break;
					case 'r':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Rook(i, j, false, "/Images/blackrook.png"));
						j++;
						break;
					case 'n':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Knight(i, j, false, "/Images/blackKnight.png"));
						j++;
						break;
					case 'b':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Bishop(i, j, false, "/Images/blackbishop.png"));
						j++;
						break;
					case 'q':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Queen(i, j, false, "/Images/blackqueen.png"));
						j++;
						break;
					case 'k':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new King(i, j, false, "/Images/blackking.png"));
						j++;
						break;

					case 'P':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Pawn(i, j, true, "/Images/whitepawn.png"));
						j++;
						break;
					case 'R':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Rook(i, j, true, "/Images/whiterook.png"));
						j++;
						break;
					case 'N':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Knight(i, j, true, "/Images/whiteknight.png"));
						j++;
						break;
					case 'B':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Bishop(i, j, true, "/Images/whitebishop.png"));
						j++;
						break;
					case 'Q':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Queen(i, j, true, "/Images/whitequeen.png"));
						j++;
						break;
					case 'K':
						Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new King(i, j, true, "/Images/whiteking.png"));
						j++;
						break;
					default:
						for(int k = 0; k < (c - '0'); k++, j++)
						{
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Empty(i, j));
						}
						break;
				}
			}

			return Board;
		}

        public Square[,] Generate()
        {
            Square[,] board = new Square[
                Constants.BoardRows, Constants.BoardCols];

            for (int row = 0; row < Constants.BoardRows; row++)
            {
                for (int col = 0; col < Constants.BoardCols; col++)
                {
                    Square square = null;
                    ChessFigure figure = GetFigure(row,col);                    

                    if ((row + col) % 2 ==0)
                    {
                        square = new Square(row, col, true, figure);
                    }
                    else
                    {
                        square = new Square(row, col, false, figure);
                    }

                    board[row, col] = square;
                }
            }

            return board;
        }

        private ChessFigure GetFigure(int row, int col)
        {
            ChessFigure figure = new Empty(row, col);

            if (row == 0)
            {
                if (col == 1 || col ==6)
                {
                    figure = new Knight(row, col, false, "/Images/blackknight.png");
                }
                if (col == 3)
                {
                    figure = new Queen(row, col, false, "/Images/blackqueen.png");
                }
            }
            if (row == 7)
            {
                if (col == 1 || col == 6)
                {
                    figure = new Knight(row, col, true, "/Images/whiteknight.png");
                }

                if (col == 3)
                {
                    figure = new Queen(row, col, true, "/Images/whitequeen.png");
                }
            }

            if (row == 1)
            {
                figure = new Pawn(row, col, false, "/Images/blackpawn.png");
            }
            if (row == 6)
            {
                figure = new Pawn(row, col, true, "/Images/whitepawn.png");
            }

            return figure;
        }

		public Square[,] Generate_from_matrix(string matrix)
		{
			Square[,] Board = Generate_from_matrix(matrix.Split('\n'));
			return Board;
		}

		public Square[,] Generate_from_matrix(string[] matrix)
		{
			Square[,] Board = new Square[matrix.GetLength(0), matrix[0].Length];

			for(int i = 0; i < matrix.GetLength(0); i++)
			{
				if (matrix[i].Length != matrix[0].Length)
				{
					MessageBox.Show("The matrix which was passed is not a rectangle. Returning default board");
					return Generate_from_fen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
				}
			}
			
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for(int j = 0; j < matrix[i].Length; j++)
				{
					char c = matrix[i][j];
					switch (c)
					{
						case 'p':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Pawn(i, j, false, "/Images/blackpawn.png"));
							break;
						case 'r':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Rook(i, j, false, "/Images/blackrook.png"));
							break;
						case 'n':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Knight(i, j, false, "/Images/blackKnight.png"));
							break;
						case 'b':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Bishop(i, j, false, "/Images/blackbishop.png"));
							break;
						case 'q':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Queen(i, j, false, "/Images/blackqueen.png"));
							break;
						case 'k':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new King(i, j, false, "/Images/blackking.png"));
							break;

						case 'P':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Pawn(i, j, true, "/Images/whitepawn.png"));
							break;
						case 'R':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Rook(i, j, true, "/Images/whiterook.png"));
							break;
						case 'N':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Knight(i, j, true, "/Images/whiteknight.png"));
							break;
						case 'B':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Bishop(i, j, true, "/Images/whitebishop.png"));
							break;
						case 'Q':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Queen(i, j, true, "/Images/whitequeen.png"));
							break;
						case 'K':
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new King(i, j, true, "/Images/whiteking.png"));
							break;
						case ' ':
							j--;
							break;
						default:
							Board[i, j] = new Square(i, j, (i + j) % 2 == 0, new Empty(i, j));
							break;
					}
				}
			}
			

			return Board;
		}
	}
}
