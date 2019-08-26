using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
	/// <summary>
	/// The debug flag is used by the programmer to test various situations
	/// specified by him and should be disabled in normal situations
	/// </summary>
	public enum GameSetting
	{
		DEBUG = (1<<0),
		WHITEBLACK = (1<<1),
		FIFTYRULE = (1<<3),
		THREEFOLDRULE = (1<<4),
		CHECKMATE = (1<<5),
		STALEMATE = (1<<6),
		PROMOTION = (1<<7),
		AUTOPROMOTION = (1<<8)
	}
}
