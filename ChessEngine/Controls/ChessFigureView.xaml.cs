using ChessEngine.Data;
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
    /// Interaction logic for ChessFigureView.xaml
    /// </summary>
    public partial class ChessFigureView : UserControl
    {
        public static readonly DependencyProperty FigureProperty =
                DependencyProperty.Register(
                      "Figure",
                       typeof(ChessFigure),
                       typeof(ChessFigureView));

        public static readonly DependencyProperty DragInitCommandProperty =
        DependencyProperty.Register(
              "DragInitCommand",
               typeof(ICommand),
               typeof(ChessFigureView));

        public static readonly DependencyProperty DragOverCommandProperty =
        DependencyProperty.Register(
              "DragOverCommand",
               typeof(ICommand),
               typeof(ChessFigureView));

        public ChessFigureView()
        {
            InitializeComponent();
        }

        public ChessFigure Figure
        {
            get
            {
                return (ChessFigure)GetValue(FigureProperty);
            }
            set
            {
                SetValue(FigureProperty, value);
            }
        }

        public ICommand DragInitCommand
        {
            get
            {
                return (ICommand)GetValue(DragInitCommandProperty);
            }
            set
            {
                SetValue(DragInitCommandProperty, value);
            }
        }

        public ICommand DragOverCommand
        {
            get
            {
                return (ICommand)GetValue(DragOverCommandProperty);
            }
            set
            {
                SetValue(DragOverCommandProperty, value);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Figure.ToString());
        }

        private void DragInitHandler(object sender, MouseButtonEventArgs e)
        {
            DragInitCommand.Execute(Figure);
        }

        private void DragOverHandler(object sender, MouseButtonEventArgs e)
        {
            DragOverCommand.Execute(Figure);
        }
    }
}
