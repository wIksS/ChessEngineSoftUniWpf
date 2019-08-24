using ChessEngine.Data;
using System;
using System.Collections.Generic;
using System.IO;
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

#if !TESTING_DRAGDROP
		private void DragInitHandler(object sender, MouseButtonEventArgs e)
        {
            DragInitCommand.Execute(Figure);
        }

        private void DragOverHandler(object sender, MouseButtonEventArgs e)
        {
            DragOverCommand.Execute(Figure);
        }
#endif


#if TESTING_DRAGDROP
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (Figure.Name != "Empty")
				{
					DragInitCommand.Execute(Figure);
					DataObject data = new DataObject();
					data.SetData("ChessFigure", FigureProperty);
					data.SetData("Object", this);

					var effects = DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
					if (effects == DragDropEffects.None)
					{
						(data.GetData("Object") as ChessFigureView).Visibility = Visibility.Visible;
					}
				}
			}
		}
		/// <summary>
		/// Lol just set the cursor to the image of the figure, why not
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			// These Effects values are set in the drop target's
			// DragOver event handler.
			if (e.Effects.HasFlag(DragDropEffects.Move))
			{
				UIElement control = FigImage;
				// convert FrameworkElement to PNG stream
				var pngStream = new MemoryStream();
				control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
				Rect rect = new Rect(0, 0, control.DesiredSize.Width, control.DesiredSize.Height);
				RenderTargetBitmap rtb = new RenderTargetBitmap((int)control.DesiredSize.Width, (int)control.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);

				control.Arrange(rect);
				rtb.Render(control);

				PngBitmapEncoder png = new PngBitmapEncoder();
				png.Frames.Add(BitmapFrame.Create(rtb));
				png.Save(pngStream);

		#region Some png shit at the begining
				var cursorStream = new MemoryStream();
				cursorStream.Write(new byte[2] { 0x00, 0x00 }, 0, 2);                        
				cursorStream.Write(new byte[2] { 0x02, 0x00 }, 0, 2);                        
				cursorStream.Write(new byte[2] { 0x01, 0x00 }, 0, 2);                        
				cursorStream.Write(new byte[1] { (byte)control.DesiredSize.Width }, 0, 1);   
				cursorStream.Write(new byte[1] { (byte)control.DesiredSize.Height }, 0, 1);  
				cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                              
				cursorStream.Write(new byte[1] { 0x00 }, 0, 1);                              
				cursorStream.Write(new byte[2] { (byte)32.0, 0x00 }, 0, 2);                   
				cursorStream.Write(new byte[2] { (byte)32.0, 0x00 }, 0, 2);                   
				cursorStream.Write(new byte[4] {                                             
                                          (byte)((pngStream.Length & 0x000000FF)),
										  (byte)((pngStream.Length & 0x0000FF00) >> 0x08),
										  (byte)((pngStream.Length & 0x00FF0000) >> 0x10),
										  (byte)((pngStream.Length & 0xFF000000) >> 0x18)
									   }, 0, 4);
				cursorStream.Write(new byte[4] {                                                    
                                          (byte)0x16,
										  (byte)0x00,
										  (byte)0x00,
										  (byte)0x00,
									   }, 0, 4);
		#endregion

				// copy PNG stream to cursor stream
				pngStream.Seek(0, SeekOrigin.Begin);
				pngStream.CopyTo(cursorStream);

				// return cursor stream
				cursorStream.Seek(0, SeekOrigin.Begin);
				Mouse.SetCursor(new Cursor(cursorStream));
			}

			//Set visibility to Hidden so that there is the illusion of the figure moving preemptively
			this.Visibility = Visibility.Hidden;

			e.Handled = true;
		}
		protected override void OnDrop(DragEventArgs e)
		{
			(e.Data.GetData("Object") as ChessFigureView).Visibility = Visibility.Visible;
			base.OnDrop(e);
			//Set visibility back to visible
			
			DragOverCommand.Execute(Figure);
			e.Handled = true;
		}
#endif
	}
}
