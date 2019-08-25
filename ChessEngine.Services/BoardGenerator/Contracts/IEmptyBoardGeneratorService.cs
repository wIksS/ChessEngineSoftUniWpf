using ChessEngine.Data;

namespace ChessEngine.Services.BoardGenerator.Contracts
{
    public interface IEmptyBoardGeneratorService
    {
        Square[,] GenerateEmptyBoard(int rows, int cols);
    }
}
