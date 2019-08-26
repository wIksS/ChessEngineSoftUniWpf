namespace ChessEngine.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using ChessEngine.Data;
    using ChessEngine.Services.BoardGenerator.Contracts;
    using ChessEngine.Services.Engine.Contracts;
    
    public class ChessGridViewModel
    {
        private readonly IBoardGeneratorService generatorService;
        private readonly IChessRulesService rulesService;
        private readonly IChessGameService GameService;

		private ICommand initCommand;
        private ICommand dragInitCommand;
        private ICommand dragOverCommand;
		private ICommand dragEnterCommand;
		private ICommand dragLeaveCommand;
		private ChessFigure selectedFigure;
        private Square[,] board;

        public ChessGridViewModel(IBoardGeneratorService generator, IChessRulesService rules, IChessGameService game)
        {
			this.GameService = game;
            this.generatorService = generator;
            this.rulesService = rules;
            this.ChessGrid = new ObservableCollection<Square>();
        }

        public ObservableCollection<Square> ChessGrid { get; set; }

        public ICommand InitCommand
        {
            get
            {
                if (initCommand == null)
                {
                    initCommand = new RelayCommand<object>(Init);
                }
                return initCommand;
            }
        }

		public ICommand DragLeaveCommand
		{
			get
			{
				if (dragLeaveCommand == null)
				{
					dragLeaveCommand = new RelayCommand<ChessFigure>(DragFigureLeave);
				}
				return dragLeaveCommand;
			}
		}

		public ICommand DragEnterCommand
		{
			get
			{
				if (dragEnterCommand == null)
				{
					dragEnterCommand = new RelayCommand<ChessFigure>(DragFigureEnter);
				}
				return dragEnterCommand;
			}
		}

		public ICommand DragInitCommand
        {
            get
            {
                if (dragInitCommand == null)
                {
                    dragInitCommand = new RelayCommand<ChessFigure>(DragFigureInit);
                }
                return dragInitCommand;
            }
        }

        public ICommand DragOverCommand
        {
            get
            {
                if (dragOverCommand == null)
                {
                    dragOverCommand = new RelayCommand<ChessFigure>(DropFigure);
                }
                return dragOverCommand;
            }
        }

		public void DragFigureEnter(ChessFigure figure)
		{
			dynamic dynamicFigure = selectedFigure;
			ChessMoveInfo MoveInfo = rulesService.Check(board, dynamicFigure, figure);
			if (MoveInfo)
			{
				board[figure.Row, figure.Col].CursorOver = true;
			}
		}

		public void DragFigureLeave(ChessFigure figure)
		{
			if(board[figure.Row, figure.Col].CursorOver)
				board[figure.Row, figure.Col].CursorOver = false;
		}

		public void Init(object data)
        {
            board = generatorService.Generate_from_fen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
            foreach (var square in board)
            {
                ChessGrid.Add(square);
            }
        }

        public void DragFigureInit(ChessFigure figure)
        {
            selectedFigure = figure;
        }

        public void DropFigure(ChessFigure figure)
        {
			DragFigureLeave(figure);

			dynamic dynamicFigure = selectedFigure;

			ChessMoveInfo MoveInfo = rulesService.Check(board, dynamicFigure, figure);

			if (GameService.Process_move(board,MoveInfo))
			{
				//TODO: implement pawn promotion
			}
		}

    }
}
