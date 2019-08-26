namespace ChessEngine.Tests.CheckmateTests
{
    using NUnit.Framework;
    using ChessEngine.Common;
    using ChessEngine.Data;
    using ChessEngine.Services;
    using ChessEngine.Services.BoardGenerator;
    
    public class NoCkeckmateTester
    {
        private Square[,] board;
        private CheckmateService service;
        private EmptyBoardGeneratorService boardGenerator;

        [SetUp]
        public void SetUp()
        {
            boardGenerator = new EmptyBoardGeneratorService();
            board = boardGenerator.GenerateEmptyBoard(Constants.BoardRows,
                                                      Constants.BoardCols);

            //This is a very stupid way but it works for now
            this.service = new CheckmateService(new ChessRulesService());
        }

        [Test]
        public void TestEscapeFromCheckWithQueenOnTheSameRow()
        {
            King king = new King(0, 0, true, "");
            Queen queen = new Queen(0, 3, false, "");
            board[0, 0].Figure = king;
            board[0, 3].Figure = queen;

            Assert.False(service.IsCheckMate(board, king));
        }

        [Test]
        public void TestEscapeFromBishop()
        {
            King king = new King(0, 0, true, "");
            Bishop bishop = new Bishop(3, 3, false, "");

            board[0, 0].Figure = king;
            board[3, 3].Figure = bishop;

            Assert.False(service.IsCheckMate(board, king));
        }

        [Test]
        public void TestEscapeFromRookWithPawnInFront()
        {
            King king = new King(0, 0, true, "");
            Rook rook = new Rook(0, 4, false, "");
            Pawn pawn = new Pawn(1, 0, false, "");

            board[0, 0].Figure = king;
            board[0, 4].Figure = rook;
            board[1, 0].Figure = pawn;

            Assert.False(service.IsCheckMate(board, king));
        }

        [Test]
        public void TestEscapeFromPawn()
        {
            King king = new King(3, 3, true, "");
            Pawn pawn = new Pawn(2, 2, false, "");

            board[3, 3].Figure = king;
            board[2, 2].Figure = pawn;

            Assert.False(service.IsCheckMate(board, king));
        }

        [Test]
        public void TestTakeAttakingQueenWithBishop()
        {
            King king = new King(0, 0, true, "");
            Queen queen = new Queen(0, 3, false, "");
            Rook rook = new Rook(1, 3, false, "");
            Bishop bishop = new Bishop(2, 5, true, "");

            board[0, 0].Figure = king;
            board[0, 3].Figure = queen;
            board[1, 3].Figure = rook;
            board[2, 5].Figure = bishop;

            Assert.False(service.IsCheckMate(board, king));
        }

        [Test]
        public void TestBlockAttackingRookWithKnight()
        {
            King king = new King(0, 0, true, "");
            Rook rook1 = new Rook(0, 3, false, "");
            Rook rook2 = new Rook(1, 3, false, "");
            Knight knight = new Knight(2, 2, true, "");

            board[0, 0].Figure = king;
            board[0, 3].Figure = rook1;
            board[1, 3].Figure = rook2;
            board[2, 2].Figure = knight;

            Assert.False(service.IsCheckMate(board, king));
        }
    }
}
