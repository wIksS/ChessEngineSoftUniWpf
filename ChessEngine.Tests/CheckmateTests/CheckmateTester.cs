namespace ChessEngine.Tests.CheckmateTests
{
    using NUnit.Framework;
    using ChessEngine.Common;
    using ChessEngine.Data;
    using ChessEngine.Services;
    using ChessEngine.Services.BoardGenerator;
    
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
            this.service = new CheckmateService(new ChessRulesService());
        }

        [Test]
        public void TestMateWithTwoRooks()
        {
            King king = new King(0, 0, true, "");
            Rook rook1 = new Rook(0, 3, false, "");
            Rook rook2 = new Rook(1, 3, false, "");

            board[0, 0].Figure = king;
            board[0, 3].Figure = rook1;
            board[1, 3].Figure = rook2;

            Assert.True(service.IsCheckMate(board, king));
        }
    }
}
