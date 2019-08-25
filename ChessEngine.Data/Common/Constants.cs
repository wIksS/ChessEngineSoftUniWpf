using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Common
{
	public enum EndCondition{
		None,
		Checkmate,
		Stalemate,
		FiftyRule,
		Threefold
	}

    public static class Constants
    {
        public const int BoardRows = 8;
        public const int BoardCols = 8;
		public const int OffBoard = -1;
    }
}
