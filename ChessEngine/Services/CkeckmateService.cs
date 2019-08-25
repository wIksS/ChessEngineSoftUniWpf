namespace ChessEngine.Services
{
    using System;
    using System.Collections.Generic;
    using ChessEngine.Common;
    using ChessEngine.Data;
    using ChessEngine.Services.Contracts;
    
    public class CkeckmateService : ICheckmateService
    {
        //private field of IChessRulesService type is used for
        //getting possible moves
        private readonly IChessRulesService rulesService;


        //private field of IKingAttackerService type is used for
        //checking is a figure attacks the king
        private readonly IKingAttackerService kingAttackerService;

        public CkeckmateService(IChessRulesService rulesService,
                                IKingAttackerService kingAttackerService)
        {
            this.rulesService = rulesService;
            this.kingAttackerService = kingAttackerService;
        }

        //the class API
        public bool IsCheckMate(Square[,] board, King attackedKing)
        {
            bool canKingMove = CheckForKingPossibleMoves(board, attackedKing);
            if (canKingMove)
            {
                return false;
            }

            List<ChessFigure> attackers = GetAttackers(board, attackedKing);

            //If there are two attackers and we already checked that the king can`t move
            //that means it`s checkmate
            if (attackers.Count == 2)
            {
                return true;
            }

            //Only attacker in the list
            ChessFigure attacker = attackers[0];

            bool canAttackerBeTaken = CheckIfAttackerCanBeTaken(board, attacker);

            if (canAttackerBeTaken)
            {
                return false;
            }

            bool canAttackerBeBlocked = CheckIfAttackerCanBeBlocked(board, attacker);
            

            return true;
        }

        private bool CheckIfAttackerCanBeBlocked(Square[,] board, ChessFigure attacker)
        {
            throw new NotImplementedException();
        }

        private bool CheckIfAttackerCanBeTaken(Square[,] board, ChessFigure attacker)
        {
            //iterate through all squares
            foreach (var square in board)
            {
                //Skip empty squares
                if (square.Figure.Name != Constants.EmptySquare)
                {
                    //Skip own figures and the oposite king
                    if (square.Figure.IsWhite != attacker.IsWhite
                        && square.Figure.Name != Constants.KingName)
                    {
                        //Check is one of our pieces can take the attacker
                        if (rulesService.Check(board, square.Figure, attacker))
                        {
                            //we return true, attacker could be taken
                            return true;
                        }
                    }
                }
            }
            //we can`t take the attacker
            return false;
        }

        //Get all figures that attack king
        //The figures could be max 2
        private List<ChessFigure> GetAttackers(Square[,] board, King attackedKing)
        {
            List<ChessFigure> attackers = new List<ChessFigure>();
            //iterate through all squares
            foreach (var square in board)
            {
                //Skip empty squares
                if (square.Figure.Name != Constants.EmptySquare)
                {
                    //Skip own figures and the oposite king
                    if (square.Figure.IsWhite != attackedKing.IsWhite
                        && square.Figure.Name != Constants.KingName)
                    {
                        //Check if current figure is attacking the king
                        //if it is we add it to the attackers list
                        if (kingAttackerService.IsKingAttacker(board,
                                                               attackedKing,
                                                               square.Figure))
                        {
                            attackers.Add(square.Figure);
                        }
                    }
                }
                //if two attackers are found we break the loop
                if (attackers.Count == 2)
                {
                    break;
                }
            }
            return attackers;
        }


        //Check if king can escape from check
        //if returns true, there is no checkmate
        private bool CheckForKingPossibleMoves(Square[,] board, King attackedKing)
        {
            int row = attackedKing.Row - 1;
            int col = attackedKing.Col - 1;
            int maxKingCountOfMoves = 8;

            for (int i = 0; i < maxKingCountOfMoves + 1; i++)
            {
                if (row >= 0 
                    && row <= 7
                    && col >= 0
                    && col <= 7)
                {
                    var move = rulesService
                    .Check(board, attackedKing, board[row, col].Figure);
                    if (move.IsAllowed)
                        return true;
                }
                
                col++;
                if (col > attackedKing.Col + 2)
                {
                    row++;
                    col = attackedKing.Col - 1;
                }
            }
            return false;
        }
    }
}
