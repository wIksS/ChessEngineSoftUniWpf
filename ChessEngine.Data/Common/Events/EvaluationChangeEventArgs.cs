using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data.Common.Events
{
    public class EvaluationChangeEventArgs : EventArgs
    {
        public EvaluationChangeEventArgs(string evaluation, string move)
        {
            this.Evaluation = evaluation;
            this.Move = move;
        }

        public string Evaluation { get; set; }

        public string Move { get; set; }
    }
}
