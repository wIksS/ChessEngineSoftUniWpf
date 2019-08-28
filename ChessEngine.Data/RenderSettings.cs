using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class RenderSettings : BasePropertyChanged
    {
        public bool RotateBoard { get; set; } = false;

        public void CopyTo(RenderSettings cpy)
        {
            cpy.RotateBoard = RotateBoard;
        }
    }
}
