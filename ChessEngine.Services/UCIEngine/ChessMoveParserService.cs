using ChessEngine.Services.UCIEngine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.UCIEngine
{
    public class ChessMoveParserService : IChessMoveParserService
    {
        private Dictionary<string, int> alphabet;

        public ChessMoveParserService()
        {
            initAlphabet();
        }

        public Tuple<int, int, int, int> ParseString(string move)
        {
            int fromCol = alphabet[move[0].ToString()]-1;
            int fromRow = 8-int.Parse(move[1].ToString());

            int toCol = alphabet[move[2].ToString()] -1;
            int toRow = 8- int.Parse(move[3].ToString());

            return new Tuple<int, int, int, int>(fromRow, fromCol, toRow, toCol);
        }

        public string CastPosition(int fromRow, int fromCol, int toRow,int toCol)
        {
            StringBuilder move = new StringBuilder();
            move.Append(GetChessChar(fromCol));
            move.Append((8-fromRow).ToString());

            move.Append(GetChessChar(toCol));
            move.Append((8-toRow).ToString());

            return move.ToString();
        }

        private string GetChessChar(int col)
        {
            return ((char)('a' + col)).ToString();
        }

        private void initAlphabet()
        {
            this.alphabet = new Dictionary<string, int>();
            for (int i = 0; i < 8; i++)
            {
                var currentChar = ((char)(97 + i)).ToString();
                alphabet.Add(currentChar, i + 1);
            }
        }
    }
}
