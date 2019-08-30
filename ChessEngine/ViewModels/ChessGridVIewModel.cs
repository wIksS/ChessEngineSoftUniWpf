namespace ChessEngine.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using ChessEngine.Data;
    using ChessEngine.Data.Common.Events;
    using ChessEngine.Services.BoardGenerator.Contracts;
    using ChessEngine.Services.Engine.Contracts;
    using ChessEngine.Services.Events.Contracts;
    using ChessEngine.Services.UCIEngine.Contracts;

    public class ChessGridViewModel
    {
        private readonly IBoardGeneratorService generatorService;
        private readonly IChessRulesService rulesService;
        private readonly IChessGameService GameService;
        private readonly IEnginePlayerService engineService;
        private readonly IChessMoveParserService parserService;

        private ICommand initCommand;
        private ICommand dragInitCommand;
        private ICommand dragOverCommand;
        private ICommand dragEnterCommand;
        private ICommand dragLeaveCommand;
        private ChessFigure selectedFigure;
        private Square[,] board;
        private bool autoplay = false;
        private readonly IEventService<SettingsChangeEventArgs> eventService;

        public ChessGridViewModel(IEventService<SettingsChangeEventArgs> eventService, IChessMoveParserService parserService, IEnginePlayerService engineService, IBoardGeneratorService generator, IChessRulesService rules, IChessGameService game)
        {
            this.eventService = eventService;
            eventService.StateChanged += SettingChangedHandler;
            this.engineService = engineService;
            engineService.InitPlayer(300);
            this.GameService = game;
            this.generatorService = generator;
            this.rulesService = rules;
            this.ChessGrid = new ObservableCollection<Square>();
            this.parserService = parserService;
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
            if (board[figure.Row, figure.Col].CursorOver)
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

        public async void DropFigure(ChessFigure figure)
        {
            string currentMove =
                parserService.CastPosition(
                    selectedFigure.Row, selectedFigure.Col,
                    figure.Row, figure.Col);

            DragFigureLeave(figure);

            dynamic dynamicFigure = selectedFigure;

            ChessMoveInfo moveInfo = rulesService.Check(board, dynamicFigure, figure);

            if (GameService.ProcessMove(board, moveInfo))
            {
                if (autoplay)
                {
                    await Autoplay(currentMove);
                }
                Tuple<int,int,int,int> engineMove = parserService.ParseString(await engineService.PlayMove(currentMove));
                dynamic fromFigure = board[engineMove.Item1, engineMove.Item2].Figure;
                dynamic toFigure = board[engineMove.Item3, engineMove.Item4].Figure;

                moveInfo = rulesService.Check(board, fromFigure, toFigure);
                GameService.ProcessMove(board, moveInfo);
            }
        }

        private async Task Autoplay(string currentMove)
        {
            while (autoplay)
            {
                Tuple<int, int, int, int> engineMove = parserService.ParseString(await engineService.PlayMove(currentMove));
                dynamic fromFigure = board[engineMove.Item1, engineMove.Item2].Figure;
                dynamic toFigure = board[engineMove.Item3, engineMove.Item4].Figure;

                var moveInfo = rulesService.Check(board, fromFigure, toFigure);
                GameService.ProcessMove(board, moveInfo);
                currentMove = parserService.CastPosition(engineMove.Item1,
                    engineMove.Item2,
                    engineMove.Item3,
                    engineMove.Item4);
            }
        }

        private void SettingChangedHandler(object sender, SettingsChangeEventArgs e)
        {
            this.autoplay = e.Autoplay;
        }
    }
}
