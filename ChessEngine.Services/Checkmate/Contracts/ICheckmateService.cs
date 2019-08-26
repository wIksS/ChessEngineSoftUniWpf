using ChessEngine.Data;

namespace ChessEngine.Services.Checkmate.Contracts
{
    public interface ICheckmateService
    {
        bool IsCheckMate(Square[,] board, King attackedKing);
    }
}
