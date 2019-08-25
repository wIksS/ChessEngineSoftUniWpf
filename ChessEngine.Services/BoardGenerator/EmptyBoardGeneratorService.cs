namespace ChessEngine.Services.BoardGenerator
{
    using ChessEngine.Data;
    using ChessEngine.Services.BoardGenerator.Contracts;
    public class EmptyBoardGeneratorService : IEmptyBoardGeneratorService
    {
        public Square[,] GenerateEmptyBoard(int rows, int cols)
        {
            Square[,] board = new Square[
                rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Square square = null;

                    if ((row + col) % 2 == 0)
                    {
                        square = new Square(row, col, true, new Empty(row, col));
                    }
                    else
                    {
                        square = new Square(row, col, false, new Empty(row, col));
                    }

                    board[row, col] = square;
                }
            }

            return board;
        }
    }
}
