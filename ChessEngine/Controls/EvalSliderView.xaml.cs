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
    /// Interaction logic for EvalSliderView.xaml
    /// </summary>
    public partial class EvalSliderView : UserControl
    {
        public static readonly DependencyProperty cp =
            DependencyProperty.Register("CPScore", typeof(int), typeof(EvalSliderView));

        public int CPScore
        {
            get => (int)GetValue(cp);
            set
            {
                SetValue(cp, value);
                BlackRect.Height = this.ActualHeight / 2 - (this.ActualHeight / 1600.0) * Math.Min(Math.Max(value, -800), 800);

                //TextCP.Text = (value / 100.0).ToString();
            }
        }

        public EvalSliderView()
        {
            InitializeComponent();
            Loaded += EvalSliderView_Loaded;
        }

        private void EvalSliderView_Loaded(object sender, RoutedEventArgs e)
        {
            //CPScore = CPScore;
        }
    }
}
