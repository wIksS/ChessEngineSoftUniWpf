using ChessEngine.Data.Common.Events;
using ChessEngine.Services.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ViewModels
{
    public class SettingsViewModel : BaseNotifyPropertyChanged
    {
        private bool autoplay= false;
        private int waitTime = 0;
        private readonly IEventService<SettingsChangeEventArgs> eventService;

        public SettingsViewModel(IEventService<SettingsChangeEventArgs> eventService)
        {
            this.eventService = eventService;
        }

        public bool Autoplay
        {
            get
            {
                return this.autoplay;
            }
            set
            {
                this.autoplay = value;
                NotifyChanged("Autoplay");
                eventService.ChangeState(new SettingsChangeEventArgs(WaitTime, Autoplay));
            }
        }

        public int WaitTime
        {
            get
            {
                return this.waitTime;
            }
            set
            {
                this.waitTime = value;
                NotifyChanged("WaitTime");
                eventService.ChangeState(new SettingsChangeEventArgs(WaitTime, Autoplay));
            }
        }
    }
}
