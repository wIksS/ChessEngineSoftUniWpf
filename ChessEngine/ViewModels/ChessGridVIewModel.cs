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
    public class ChessGridViewModel
    {
        private readonly IBoardGeneratorService generatorService;
        private readonly IChessRulesService rulesService;

        private ICommand initCommand;
        private ICommand dragInitCommand;
        private ICommand dragOverCommand;
        private ChessFigure selectedFigure;
        private Square[,] board;

        public ChessGridViewModel(IBoardGeneratorService generator, IChessRulesService rules)
        {
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

        public ICommand DragInitCommand
        {
            get
            {
                if (dragInitCommand == null)
                {
                    dragInitCommand = new RelayCommand<ChessFigure>(DragInit);
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
                    dragOverCommand = new RelayCommand<ChessFigure>(DragOver);
                }
                return dragOverCommand;
            }
        }

        public void Init(object data)
        {
            board = generatorService.Generate();
            foreach (var square in board)
            {
                ChessGrid.Add(square);
            }
        }

        public void DragInit(ChessFigure figure)
        {
            selectedFigure = figure;
        }

        public void DragOver(ChessFigure figure)
        {            
            dynamic dynamicFigure = selectedFigure;
            if (!rulesService.Check(board, dynamicFigure, figure))
            {
                return;
            }
            var selectedSquare = board[selectedFigure.Row, selectedFigure.Col];
            var destinationSquare = board[figure.Row, figure.Col];

            var selectedSquareIndex = ChessGrid.IndexOf(selectedSquare);
            var destinationSquareIndex = ChessGrid.IndexOf(destinationSquare);
            var newSelectedSquare =
                new Square(selectedSquare.Row, selectedSquare.Col, selectedSquare.IsWhite,
                new Empty(selectedSquare.Row, selectedSquare.Col));
            ChessGrid[selectedSquareIndex] = newSelectedSquare;

            selectedFigure.Row = destinationSquare.Row;
            selectedFigure.Col = destinationSquare.Col;
            var newDestinationSquare = new Square(destinationSquare.Row, destinationSquare.Col, destinationSquare.IsWhite,
                selectedFigure);
            ChessGrid[destinationSquareIndex] = newDestinationSquare;

            board[selectedSquare.Row, selectedSquare.Col] = newSelectedSquare;
            board[destinationSquare.Row, destinationSquare.Col] = newDestinationSquare;
        }

    }
}
