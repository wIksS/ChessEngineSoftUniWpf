using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using ChessEngine.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Events;
using ChessEngine.EventAggregatorNamespace;

namespace ChessEngine.ViewModels
{
    public class ChessGridViewModel : BasePropertyChanged
    {
		private RenderSettings renderSettings;
		private UserPreferences userPreferences;

        private readonly IBoardGeneratorService generatorService;
        private readonly IChessGameService gameService;
        private readonly IEventAggregator eventAggregator;
        private readonly IUCIEngineService engineService;
        private readonly IBoardParserService boardParser;
        private UCIListener listener;

        private ICommand initCommand;
        private ICommand dragInitCommand;
        private ICommand dragOverCommand;
		private ICommand dragEnterCommand;
		private ICommand dragLeaveCommand;
		private ChessFigure selectedFigure;
        private Square[,] board;

		public bool WhiteToMove { get; set; }

		public bool LockBoard { get; set; }

		public ChessGridViewModel(IBoardGeneratorService generator, IChessRulesService rules, IChessGameService game, IEventAggregator eventAggregator, IUCIEngineService engineService, IBoardParserService boardParser)
        {
            this.boardParser = boardParser;
            this.listener = new UCIListener();
            this.listener.PropertyChanged += Listener_PropertyChanged;
            this.engineService = engineService;
            this.engineService.Init(5000);
            this.engineService.AddListener(listener);

            this.eventAggregator = eventAggregator;
            this.gameService = game;
			this.LockBoard = false;
            this.renderSettings = new RenderSettings();
            this.userPreferences = new UserPreferences();

			this.WhiteToMove = gameService.White_to_move();
			this.generatorService = generator;
            this.ChessGrid = new ObservableCollection<Square>();
            this.ReverseChessGrid = new ObservableCollection<Square>();
            this.WhitePromotionFigures = new ObservableCollection<PromotionItem>();
            this.BlackPromotionFigures = new ObservableCollection<PromotionItem>();


            this.eventAggregator.GetEvent<PreferenceChanges>().Subscribe(HandlePreferenceChanges);
            this.eventAggregator.GetEvent<SettingChanges>().Subscribe(HandleSettingChanges);
            this.eventAggregator.GetEvent<RenderChanges>().Subscribe(HandleRenderChanges);
        }

        private void Listener_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "CurrentCPScore")
            {
                if(WhiteToMove)eventAggregator.GetEvent<EvalChanges>().Publish(listener.CurrentCPScore);
                else eventAggregator.GetEvent<EvalChanges>().Publish(-listener.CurrentCPScore);
            }
        }

        private void HandleRenderChanges(RenderSettings obj)
        {
            if (renderSettings.RotateBoard != obj.RotateBoard)
            {
                ReverseChessGrid.Clear();
                for (int i = ChessGrid.Count - 1; i >= 0; i--) ReverseChessGrid.Add(ChessGrid[i]);
            }
            obj.CopyTo(renderSettings);
        }

        private void HandleSettingChanges(GameSettings obj)
        {
            obj.CopyTo(gameService.GetGameSetting());
        }

        private void HandlePreferenceChanges(UserPreferences obj)
        {
            obj.CopyTo(userPreferences);
        }

        public ObservableCollection<Square> ChessGrid { get; set; }
        public ObservableCollection<Square> ReverseChessGrid { get; set; }
        public ObservableCollection<PromotionItem> WhitePromotionFigures { get; set; }
        public ObservableCollection<PromotionItem> BlackPromotionFigures { get; set; }
		
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
			ChessMoveInfo MoveInfo = gameService.Check(board, dynamicFigure, figure);
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
            //board = generatorService.Generate_from_matrix(
            //	"********\n" +
            //	"***PP***\n" +
            //	"********\n" +
            //	"********\n" +
            //	"********\n" +
            //	"********\n" +
            //	"***pp***\n" +
            //	"********");
            foreach (var square in board)
            {
                ChessGrid.Add(square);
            }
			if (renderSettings.RotateBoard)
				for (int i = ChessGrid.Count - 1; i >= 0; i--) ReverseChessGrid.Add(ChessGrid[i]);
			else
                for (int i = 0; i < ChessGrid.Count; i++) ReverseChessGrid.Add(ChessGrid[i]);

            for (int i = 0; i < Constants.BoardCols; i++)
			{
				WhitePromotionFigures.Add(new PromotionItem());
				BlackPromotionFigures.Add(new PromotionItem());
			}
		}

		public void DragFigureInit(ChessFigure figure)
        {
            selectedFigure = figure;
        }

        public async void DropFigure(ChessFigure figure)
        {
			DragFigureLeave(figure);

			dynamic dynamicFigure = selectedFigure;

			ChessMoveInfo MoveInfo = gameService.Check(board, dynamicFigure, figure);

            if (MoveInfo.IsPromotion && gameService.GetGameSetting().Promotion)
			{
                if (userPreferences.AutoPromotion)
                {
                    if (MoveInfo.MovedFigureIsWhite)
                    {
                        board[selectedFigure.Row, selectedFigure.Col].Figure = new Queen(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, "/Images/whitequeen.png");
                    }
                    else
                    {
                        board[selectedFigure.Row, selectedFigure.Col].Figure = new Queen(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, "/Images/blackqueen.png");
                    }
                }
                else
                {
                    this.LockBoard = true;
                    if (MoveInfo.MovedFigureIsWhite)
                    {
                        WhitePromotionFigures[MoveInfo.ToCol].Figure = selectedFigure;
                        WhitePromotionFigures[MoveInfo.ToCol].IsVisible = true;
                        try
                        {
                            await Task.Delay(1000000, WhitePromotionFigures[MoveInfo.ToCol].PromotionToken);
                        }
                        catch
                        {
                            if (selectedFigure.Name == "Queen")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Queen(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Rook")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Rook(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Bishop")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Bishop(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Knight")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Knight(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                        }
                        WhitePromotionFigures[MoveInfo.ToCol].IsVisible = false;
                        WhitePromotionFigures[MoveInfo.ToCol].Figure = new Empty(1, 1);
                    }
                    else
                    {
                        if (!renderSettings.RotateBoard)
                        {
                            BlackPromotionFigures[MoveInfo.ToCol].Figure = selectedFigure;
                            BlackPromotionFigures[MoveInfo.ToCol].IsVisible = true;
                        }
                        else
                        {
                            BlackPromotionFigures[7 - MoveInfo.ToCol].Figure = selectedFigure;
                            BlackPromotionFigures[7 - MoveInfo.ToCol].IsVisible = true;
                        }

                        try
                        {
                            if (!renderSettings.RotateBoard)

                                await Task.Delay(1000000, BlackPromotionFigures[MoveInfo.ToCol].PromotionToken);
                            else

                                await Task.Delay(1000000, BlackPromotionFigures[7 - MoveInfo.ToCol].PromotionToken);
                        }
                        catch
                        {
                            if (selectedFigure.Name == "Queen")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Queen(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Rook")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Rook(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Bishop")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Bishop(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                            else if (selectedFigure.Name == "Knight")
                            {
                                board[selectedFigure.Row, selectedFigure.Col].Figure = new Knight(selectedFigure.Row, selectedFigure.Col, selectedFigure.IsWhite, selectedFigure.Image);
                            }
                        }
                        if (!renderSettings.RotateBoard)
                        {
                            BlackPromotionFigures[MoveInfo.ToCol].IsVisible = false;
                            BlackPromotionFigures[MoveInfo.ToCol].Figure = new Empty(1, 1);
                        }
                        else
                        {
                            BlackPromotionFigures[7 - MoveInfo.ToCol].IsVisible = false;
                            BlackPromotionFigures[7 - MoveInfo.ToCol].Figure = new Empty(1, 1);
                        }
                    }
                    this.LockBoard = false;
                }
			}

			if (gameService.Process_move(board, MoveInfo))
            {
                this.WhiteToMove = gameService.White_to_move();

                gameService.Check_for_end_condition(board, false);
                gameService.Check_for_end_condition(board, true);

                if (userPreferences.ShowStockfishEval)
                {
                    engineService.AddMove(boardParser.MoveParserUCI(MoveInfo));
                    string currFen = gameService.Get_full_fen(board);
                    engineService.EvalPositionDepth(currFen,20);
                }
            }
			
		}

    }
}
