using ChessEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;

namespace ChessEngine.Views
{
    /// <summary>
    /// Interaction logic for ChessGridView.xaml
    /// </summary>
    public partial class ChessGridView : Page
    {
        public ChessGridView()
        {
            InitializeComponent();
            this.DataContext = 
                Bootstraper.Container.Resolve<ChessGridViewModel>();
        }
    }
}
