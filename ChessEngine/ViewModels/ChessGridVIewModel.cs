using ChessEngine.Common;
using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChessEngine.ViewModels
{
    public class ChessGridViewModel : BasePropertyChanged
    {
		private GameMode MODE;

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

        public ChessGridViewModel(IBoardGeneratorService generator, IChessRulesService rules, IChessGameService game)
        {
			this.MODE |= GameMode.ROTATION;
			this.GameService = game;
			this.WhiteToMove = GameService.White_to_move();
			this.generatorService = generator;
            this.ChessGrid = new ObservableCollection<Square>();
            this.ReverseChessGrid = new ObservableCollection<Square>();
		}

        public ObservableCollection<Square> ChessGrid { get; set; }
        public ObservableCollection<Square> ReverseChessGrid { get; set; }

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
            board = generatorService.Generate_from_fen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
            foreach (var square in board)
            {
                ChessGrid.Add(square);
            }
			if ((MODE & GameMode.ROTATION) == GameMode.ROTATION)
				for (int i = ChessGrid.Count - 1; i >= 0; i--) ReverseChessGrid.Add(ChessGrid[i]);
			else
				ReverseChessGrid = ChessGrid;
		}

		public void DragFigureInit(ChessFigure figure)
        {
            selectedFigure = figure;
        }

        public void DropFigure(ChessFigure figure)
        {
			DragFigureLeave(figure);

			dynamic dynamicFigure = selectedFigure;

			ChessMoveInfo MoveInfo = GameService.Check(board, dynamicFigure, figure);

			if (GameService.Process_move(board,MoveInfo))
			{
				//TODO: implement pawn promotion
			}

			this.WhiteToMove = GameService.White_to_move();

			GameService.Check_for_end_condition(board, false);
			GameService.Check_for_end_condition(board, true);
		}

    }
}
