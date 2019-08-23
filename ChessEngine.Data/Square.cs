using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class Square : BasePositionModel
    {
        public Square(int row, int col, bool isWhite, ChessFigure figure)
        {
            this.Row = row;
            this.Col = col;
            this.IsWhite = isWhite;
            this.Figure = figure;
        }

        public bool IsWhite { get; set; }

        public ChessFigure Figure { get; set; }
    }
}
