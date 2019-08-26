using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChessEngine.ViewModels
{
    public class ChessGridViewModel : BasePropertyChanged
    {
		private RenderMode MODE;

        private readonly IBoardGeneratorService generatorService;
        private readonly IChessGameService GameService;

		private ICommand initCommand;
        private ICommand dragInitCommand;
        private ICommand dragOverCommand;
		private ICommand dragEnterCommand;
		private ICommand dragLeaveCommand;
		private ChessFigure selectedFigure;
        private Square[,] board;

		public bool WhiteToMove { get; set; }

		public bool LockBoard { get; set; }

		public ChessGridViewModel(IBoardGeneratorService generator, IChessRulesService rules, IChessGameService game)
        {
			this.GameService = game;
			this.LockBoard = false;
			//this.MODE |= RenderMode.ROTATION;

			//GameService.EnableGameSetting(GameSetting.WHITEBLACK);
			//GameService.EnableGameSetting(GameSetting.THREEFOLDRULE);
			//GameService.EnableGameSetting(GameSetting.STALEMATE);
			//GameService.EnableGameSetting(GameSetting.PROMOTION);
			//GameService.EnableGameSetting(GameSetting.AUTOPROMOTION);
			//GameService.EnableGameSetting(GameSetting.CHECKMATE);
			//GameService.EnableGameSetting(GameSetting.FIFTYRULE);

			this.WhiteToMove = GameService.White_to_move();
			this.generatorService = generator;
            this.ChessGrid = new ObservableCollection<Square>();
            this.ReverseChessGrid = new ObservableCollection<Square>();
            this.WhitePromotionFigures = new ObservableCollection<PromotionItem>();
            this.BlackPromotionFigures = new ObservableCollection<PromotionItem>();
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
			ChessMoveInfo MoveInfo = GameService.Check(board, dynamicFigure, figure);
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
			//board = generatorService.Generate_from_fen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
			board = generatorService.Generate_from_matrix(
				"********\n" +
				"***PP***\n" +
				"********\n" +
				"********\n" +
				"********\n" +
				"********\n" +
				"***pp***\n" +
				"********");
            foreach (var square in board)
            {
                ChessGrid.Add(square);
            }
			if ((MODE & RenderMode.ROTATION) == RenderMode.ROTATION)
				for (int i = ChessGrid.Count - 1; i >= 0; i--) ReverseChessGrid.Add(ChessGrid[i]);
			else
				ReverseChessGrid = ChessGrid;

			for(int i = 0; i < Constants.BoardCols; i++)
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

			ChessMoveInfo MoveInfo = GameService.Check(board, dynamicFigure, figure);

			if (MoveInfo.IsPromotion)
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
						if(selectedFigure.Name == "Queen")
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
					WhitePromotionFigures[MoveInfo.ToCol].Figure = new Empty(1,1);
				}
				else
				{
					BlackPromotionFigures[MoveInfo.ToCol].Figure = selectedFigure;
					BlackPromotionFigures[MoveInfo.ToCol].IsVisible = true;
					try
					{
						await Task.Delay(1000000, BlackPromotionFigures[MoveInfo.ToCol].PromotionToken);
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
					BlackPromotionFigures[MoveInfo.ToCol].IsVisible = false;
					BlackPromotionFigures[MoveInfo.ToCol].Figure = new Empty(1, 1);
				}
				this.LockBoard = false;
			}

			if (GameService.Process_move(board,MoveInfo))

			this.WhiteToMove = GameService.White_to_move();
			
			GameService.Check_for_end_condition(board, false);
			GameService.Check_for_end_condition(board, true);
		}

    }
}
