using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public abstract class BaseModel : BasePropertyChanged
	{
        public int Id { get; set; }
    }
}
