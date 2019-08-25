using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
	public enum GameMode
	{
		DEBUG = (1<<0),
		WHITEBLACK = (1<<1),
		ROTATION = (1<<2)
	}
}
