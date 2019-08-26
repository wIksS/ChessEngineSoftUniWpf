using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
	/// Interaction logic for PromotionWhiteView.xaml
	/// </summary>
	public partial class PromotionWhiteView : UserControl
	{
		// The pawn which is being promoted
		public static readonly DependencyProperty FigureProperty =
			DependencyProperty.Register("Figure", typeof(ChessFigure), typeof(PromotionWhiteView));
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register( "Source", typeof(CancellationTokenSource), typeof(PromotionWhiteView));
		#region property_get_set
		public ChessFigure Figure
		{
			get{ if (GetValue(FigureProperty) == null) return new Empty(1, 1); else return (ChessFigure)GetValue(FigureProperty); }
			set{SetValue(FigureProperty, value);}
		}
		public CancellationTokenSource Source
		{
			get { return (CancellationTokenSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}
		#endregion

		public PromotionWhiteView()
		{
			InitializeComponent();
		}

		private void QueenClick(object sender, RoutedEventArgs e)
		{
			Figure.Image = "/Images/whitequeen.png";
			Figure.Name = "Queen";

			Source.Cancel();
		}
		private void RookClick(object sender, RoutedEventArgs e)
		{
			Figure.Image = "/Images/whiterook.png";
			Figure.Name = "Rook";

			Source.Cancel();
		}
		private void BishopClick(object sender, RoutedEventArgs e)
		{
			Figure.Image = "/Images/whitebishop.png";
			Figure.Name = "Bishop";

			Source.Cancel();
		}
		private void KnightClick(object sender, RoutedEventArgs e)
		{
			Figure.Image = "/Images/whiteknight.png";
			Figure.Name = "Knight";

			Source.Cancel();
		}
	}
}
