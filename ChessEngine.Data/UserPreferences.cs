using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
    public class UserPreferences : BasePropertyChanged
    {
        public bool AutoPromotion { get; set; } = false;
        public bool ShowStockfishEval { get; set; } = true;

        public void CopyTo(UserPreferences cpy)
        {
            cpy.AutoPromotion = AutoPromotion;
            cpy.ShowStockfishEval = ShowStockfishEval;
        }
    }
}
