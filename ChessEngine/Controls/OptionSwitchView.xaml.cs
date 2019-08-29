using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessEngine.Controls
{
    /// <summary>
    /// Interaction logic for OptionSwitchView.xaml
    /// </summary>
    public partial class OptionSwitchView : UserControl
    {
        private static readonly DependencyProperty setting =
        DependencyProperty.Register(
              "Setting",
               typeof(string),
               typeof(OptionSwitchView));

        private static readonly DependencyProperty clickedCommand =
        DependencyProperty.Register(
              "ClickedCommand",
               typeof(ICommand),
               typeof(OptionSwitchView));

        private static readonly DependencyProperty header =
        DependencyProperty.Register(
              "Header",
               typeof(string),
               typeof(OptionSwitchView));

        private static readonly DependencyProperty isEnabled =
        DependencyProperty.Register(
              "SettingIsTrue",
               typeof(bool),
               typeof(OptionSwitchView));

        public bool SettingIsTrue
        {
            get
            {
                return (bool)GetValue(isEnabled);
            }
            set
            {
                SetValue(isEnabled, value);
            }
        }


        public string Header
        {
            get
            {
                return (string)GetValue(header);
            }
            set
            {
                SetValue(header, value);
            }
        }

        
        public OptionSwitchView()
        {
            InitializeComponent();
            Loaded += OptionSwitchView_Loaded;
            
        }

        private void OptionSwitchView_Loaded(object sender, RoutedEventArgs e)
        {
            toggleswitch.Header = Header;
            toggleswitch.IsChecked = SettingIsTrue;
        }

        public string Setting
        {
            get
            {
                return (string)GetValue(setting);
            }
            set
            {
                SetValue(setting, value);
            }
        }

        public ICommand ClickedCommand
        {
            get
            {
                return (ICommand)GetValue(clickedCommand);
            }
            set
            {
                SetValue(clickedCommand, value);
            }
        }

        private void toggleswitch_Click(object sender, RoutedEventArgs e)
        {
            ClickedCommand.Execute(Setting);
        }
    }
}
