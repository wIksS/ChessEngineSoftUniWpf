using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class Empty : ChessFigure
    {
        public Empty(int row, int col)
            : base(row, col, false, "")
        {
            this.Name = "Empty";
        }
    }
}
