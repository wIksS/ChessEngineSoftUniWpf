namespace ChessEngine.Services
{
    using ChessEngine.Data;
    using ChessEngine.Services.Contracts;

    public class KingAttackerService : IKingAttackerService
    {
        private IChessRulesService ruleService;

        public KingAttackerService(IChessRulesService ruleService)
        {
            this.ruleService = ruleService;
        }
        public bool IsKingAttacker(Square[,] board, King king, ChessFigure figure)
        {
            ChessMoveInfo move = null;
            if (figure is Pawn pawn)
            {
                move = ruleService.Check(board, pawn, king);
            }
            else if (figure is Bishop bishop)
            {
                move = ruleService.Check(board, bishop, king);
            }
            else if (figure is Knight knight)
            {
                move = ruleService.Check(board, knight, king);
            }
            else if (figure is Rook rook)
            {
                move = ruleService.Check(board, rook, king);
            }
            else if (figure is Queen queen)
            {
                move = ruleService.Check(board, queen, king);
            }

            return move.IsAllowed;
        }
    }
}
