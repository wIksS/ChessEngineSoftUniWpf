using ChessEngine.Data;
using ChessEngine.EventAggregatorNamespace;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ViewModels
{
    public class EngineEvalViewModel : BasePropertyChanged
    {
        private readonly IEventAggregator eventAggregator;

        public int CP { get; set; } = 123;

        public bool IsEnabled { get; set; } = true;
        private bool wasEnabled;

        public EngineEvalViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<EvalChanges>().Subscribe(ChangeCP);
            eventAggregator.GetEvent<PreferenceChanges>().Subscribe(ChangeEnable);
            eventAggregator.GetEvent<SettingChanges>().Subscribe(ChangeSettings);
        }

        private void ChangeSettings(GameSettings obj)
        {
            if (!obj.WhiteBlack)
            {
                wasEnabled = IsEnabled;
                IsEnabled = false;
            }
            else
            {
                IsEnabled = wasEnabled;
            }
        }

        private void ChangeEnable(UserPreferences obj)
        {
            IsEnabled = obj.ShowStockfishEval;
        }

        public void ChangeCP(int newCP)
        {
            CP = newCP;
        }
    }
}
