using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data.Common.Events
{
    public class SettingsChangeEventArgs : EventArgs
    {
        public SettingsChangeEventArgs(int waitTime, bool autoplay)
        {
            this.WaitTime = waitTime;
            this.Autoplay = autoplay;
        }

        public int WaitTime { get; set; }

        public bool Autoplay { get; set; }
    }
}
