using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public abstract class BasePositionModel : BaseModel
    {
        public int Row { get; set; }

        public int Col { get; set; }
    }
}
