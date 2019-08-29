using ChessEngine.Data;
using ChessEngine.EventAggregatorNamespace;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChessEngine.ViewModels
{
    public class MenuViewModel
    {
        private readonly IEventAggregator eventAggregator;

        private ICommand preferenceChangeCommand;
        private ICommand gameSettingsChangeCommand;
        private ICommand renderSettingsChangeCommand;

        public GameSettings DummyGameSettings { get; set; }
        public UserPreferences DummyPreferences { get; set; }
        public RenderSettings DummyRenderSettings { get; set; }
        
        public MenuViewModel(IEventAggregator eventAggregator)
        {
            DummyPreferences = new UserPreferences();
            DummyRenderSettings = new RenderSettings();
            DummyGameSettings = new GameSettings();

            this.eventAggregator = eventAggregator;

            SendGameSettings();
            SendRenderSettings();
            SendPreferences();
        }

        private void SendPreferences()
        {
            eventAggregator.GetEvent<PreferenceChanges>().Publish(DummyPreferences);
        }

        private void SendRenderSettings()
        {
            eventAggregator.GetEvent<RenderChanges>().Publish(DummyRenderSettings);
        }

        private void SendGameSettings()
        {
            eventAggregator.GetEvent<SettingChanges>().Publish(DummyGameSettings);
        }

        public ICommand PreferenceChangeCommand
        {
            get
            {
                if (preferenceChangeCommand == null) preferenceChangeCommand = new RelayCommand<string>(HandlePreferenceChange);
                return preferenceChangeCommand;
            }
        }
        
        public ICommand GameSettingsChangeCommand
        {
            get
            {
                if (gameSettingsChangeCommand == null) gameSettingsChangeCommand = new RelayCommand<string>(HandleGameSettingsChange);
                return gameSettingsChangeCommand;
            }
        }
        
        public ICommand RenderSettingsChangeCommand
        {
            get
            {
                if (renderSettingsChangeCommand == null) renderSettingsChangeCommand = new RelayCommand<string>(HandleRenderSettingsChange);
                return renderSettingsChangeCommand;
            }
        }

        public void HandlePreferenceChange(string data)
        {
            switch (data)
            {
                case "Autopromotion":
                    DummyPreferences.AutoPromotion = !DummyPreferences.AutoPromotion;
                    break;
                case "StockfishEval":
                    DummyPreferences.ShowStockfishEval = !DummyPreferences.ShowStockfishEval;
                    break;
                default:
                    break;
            }

            SendPreferences();
        }
        public void HandleGameSettingsChange(string data)
        {

            switch (data)
            {
                case "WhiteBlack":
                    DummyGameSettings.WhiteBlack = !DummyGameSettings.WhiteBlack;
                    break;
                case "Promotion":
                    DummyGameSettings.Promotion = !DummyGameSettings.Promotion;
                    break;
                case "Threefold":
                    DummyGameSettings.Threefold = !DummyGameSettings.Threefold;
                    break;
                case "Fiftyrule":
                    DummyGameSettings.FiftyRule = !DummyGameSettings.FiftyRule;
                    break;
                case "Stalemate":
                    DummyGameSettings.Stalemate = !DummyGameSettings.Stalemate;
                    break;
                case "Checkmate":
                    DummyGameSettings.Checkmate = !DummyGameSettings.Checkmate;
                    break;
                default:
                    break;
            }
            SendGameSettings();
        }
        public void HandleRenderSettingsChange(string data)
        {
            switch (data)
            {
                case "Rotate":
                    DummyRenderSettings.RotateBoard = !DummyRenderSettings.RotateBoard;
                    break;
                default:
                    break;
            }
            SendRenderSettings();
        }
    }
}
