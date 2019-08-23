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
    public class BoardGeneratorService : IBoardGeneratorService
    {
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
    }
}
