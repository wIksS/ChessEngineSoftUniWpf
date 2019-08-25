namespace ChessEngine.Services.Contracts
{
    using ChessEngine.Data;
    public interface IKingAttackerService
    {
        bool IsKingAttacker(Square[,] board, King king, ChessFigure figure);
    }
}
