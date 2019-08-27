namespace ChessEngine.Services.Checkmate
{
    using System;
    using System.Collections.Generic;
    using ChessEngine.Common;
    using ChessEngine.Data;
    using ChessEngine.Services.Checkmate.Contracts;
    using ChessEngine.Services.Engine.Contracts;

    public class CheckmateService : ICheckmateService
    {
        //private field of IChessRulesService type is used for
        //getting possible moves
        private readonly IChessRulesService rulesService;

        public CheckmateService(IChessRulesService rulesService)
        {
            this.rulesService = rulesService;
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

            bool canAttackerBeBlocked = CheckIfAttackerCanBeBlocked(board, attackedKing, attacker);


            return true;
        }

        private bool CheckIfAttackerCanBeBlocked(Square[,] board, King attackedKing, ChessFigure attacker)
        {
            bool canBeBlocked = false;
            if (attacker.Name == Constants.PawnName
                || attacker.Name == Constants.KnightName)
            {
                return canBeBlocked;
            }

            List<Square> attackingPath = GetAttakingPath(board, attackedKing, attacker);

            if (attackingPath.Count == 0)
            {
                return canBeBlocked;
            }

            canBeBlocked = CanAttackerBeBlocked(board, attackingPath, attackedKing);

            return canBeBlocked;
        }

        private bool CanAttackerBeBlocked(Square[,] board, List<Square> attackingPath, King attackedKing)
        {
            bool canBeBlocked = false;

            foreach (var boardSquare in board)
            {
                if (boardSquare.Figure.Name != Constants.EmptySquare)
                {
                    if (boardSquare.Figure.IsWhite == attackedKing.IsWhite && boardSquare.Figure.Name != Constants.KingName)
                    {
                        foreach (var attackingPathSquare in attackingPath)
                        {
                            if (rulesService.Check(board, boardSquare.Figure, attackingPathSquare.Figure))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return canBeBlocked;
        }

        private List<Square> GetAttakingPath(Square[,] board, King attackedKing, ChessFigure attacker)
        {
            List<Square> path = new List<Square>();
            if (attacker is Rook rook)
            {
                path = GetRookPath(board, attackedKing, attacker);
            }
            else if (attacker is Bishop bishop)
            {
                path = GetBishopPath(board, attackedKing, attacker);
            }
            else if (attacker is Queen queen)
            {
                path = GetQueenPath(board, attackedKing, attacker);
            }
            else
            {
                throw new ArgumentException("Wrong attacking figure!");
            }

            return path;
        }

        private List<Square> GetQueenPath(Square[,] board, King attackedKing, ChessFigure attacker)
        {
            List<Square> path = new List<Square>();
            if (attacker.Col != attackedKing.Col && attacker.Row != attackedKing.Row)
            {
                path = GetBishopPath(board, attackedKing, attacker);
            }
            else
            {
                path = GetRookPath(board, attackedKing, attacker);
            }
            return path;
        }

        private List<Square> GetBishopPath(Square[,] board, King attackedKing, ChessFigure bishop)
        {
            List<Square> path = new List<Square>();
            //When bishop attacks the king, the always make a square
            int countOfSquares = Math.Max(attackedKing.Col, bishop.Col) - Math.Min(attackedKing.Col, bishop.Col) - 1;
            if (countOfSquares < 1)
            {
                return path;
            }
            int col = -1;
            int row = -1;
            if (attackedKing.Col < bishop.Col && attackedKing.Row < bishop.Row)
            {
                col = attackedKing.Col + 1;
                row = attackedKing.Row + 1;

            }
            else if (attackedKing.Col < bishop.Col && attackedKing.Row > bishop.Row)
            {
                col = attackedKing.Col + 1;
                row = bishop.Row + 1;
            }
            else if (attackedKing.Col > bishop.Col && attackedKing.Row > bishop.Row)
            {
                col = bishop.Col + 1;
                row = bishop.Row + 1;
            }
            else if (attackedKing.Col > bishop.Col && attackedKing.Row < bishop.Row)
            {
                col = bishop.Col + 1;
                row = attackedKing.Row + 1;
            }
            if (col != -1 && row != -1)
            {
                for (int i = 0; i < countOfSquares; i++)
                {
                    path.Add(board[row++, col++]);
                }
            }
            return path;
        }

        private List<Square> GetRookPath(Square[,] board, King attackedKing, ChessFigure rook)
        {
            List<Square> path = new List<Square>();
            if (attackedKing.Row == rook.Row)
            {
                int row = attackedKing.Row;
                if (attackedKing.Col < rook.Col)
                {
                    for (int i = attackedKing.Col + 1; i < rook.Col; i++)
                    {
                        path.Add(board[row, i]);
                    }
                }
                else
                {
                    for (int i = rook.Col + 1; i < attackedKing.Col; i++)
                    {
                        path.Add(board[row, i]);
                    }
                }
            }
            else
            {
                int col = attackedKing.Col;
                if (attackedKing.Row < rook.Row)
                {
                    for (int i = attackedKing.Row + 1; i < rook.Row; i++)
                    {
                        path.Add(board[i, col]);
                    }
                }
                else
                {
                    for (int i = rook.Row + 1; i < attackedKing.Row; i++)
                    {
                        path.Add(board[i, col]);
                    }
                }
            }

            return path;
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
                    if (square.Figure.IsWhite != attacker.IsWhite)
                    {
                        //Check is one of our pieces can take the attacker

                        
                        dynamic attackerFigure = attacker;
                        dynamic protectingFigure = square.Figure;
                        if (rulesService.Check(board, protectingFigure, attackerFigure))
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

                        //Some indian style
                        dynamic fig = square.Figure;
                        if (rulesService.Check(board, fig, (King)attackedKing))
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
                if (col > attackedKing.Col + 1)
                {
                    row++;
                    col = attackedKing.Col - 1;
                }
            }
            return false;
        }
    }
}
