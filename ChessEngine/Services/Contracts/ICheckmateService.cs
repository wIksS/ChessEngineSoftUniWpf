using ChessEngine.Data;

namespace ChessEngine.Services.Contracts
{
    public interface ICheckmateService
    {
        bool IsCheckMate(Square[,] board, King attackedKing);
    }
}
