using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.Contracts
{
    public interface IBoardGeneratorService
    {
        Square[,] Generate();
        Square[,] Generate_from_fen(string fen);
	}
}
