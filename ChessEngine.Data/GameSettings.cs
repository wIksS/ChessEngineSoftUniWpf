using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class GameSettings : BasePropertyChanged
    {
        public bool WhiteBlack { get; set; } = true;
        public bool Threefold { get; set; } = true;
        public bool FiftyRule { get; set; } = true;
        public bool Checkmate { get; set; } = true;
        public bool Stalemate { get; set; } = true;
        public bool Promotion { get; set; } = true;

        public void CopyTo(GameSettings cpy)
        {
            cpy.WhiteBlack = WhiteBlack;
            cpy.Threefold = Threefold;
            cpy.FiftyRule = FiftyRule;
            cpy.Checkmate = Checkmate;
            cpy.Stalemate = Stalemate;
            cpy.Promotion = Promotion;
        }
    }
}
