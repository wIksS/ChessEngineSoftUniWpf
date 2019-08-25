namespace ChessEngine.Tests
{
    using ChessEngine.Common;
    using ChessEngine.Data;
    using ChessEngine.Services;
    using ChessEngine.Services.BoardGenerator;
    using NUnit.Framework;
    public class CheckmateTester
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
            this.service = new CheckmateService(new ChessRulesService(),
                                                new KingAttackerService(new ChessRulesService()));
        }

        [Test]
        public void TestEscapeFromMate()
        {
            King king = new King(0, 0, true, "");
            Queen queen = new Queen(0, 3, false, "");
            board[0, 0].Figure = king;
            board[0, 3].Figure = queen;

            Assert.False(service.IsCheckMate(board, king));
        }
    }
}
