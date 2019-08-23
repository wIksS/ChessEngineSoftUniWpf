using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class Knight : ChessFigure
    {
        public Knight(int row, int col, bool isWhite, string image)
            :base(row, col, isWhite, image)
        {
            this.Name = "Knight";
        }
    }
}
