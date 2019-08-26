using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
	public class PromotionItem : BasePropertyChanged
	{
		public ChessFigure Figure { get; set; }
		public bool IsVisible { get; set; }
		public CancellationTokenSource PromotionSource { get; set; }
		public CancellationToken PromotionToken { get; set; }
		public PromotionItem()
		{
			IsVisible = false;
			PromotionSource = new CancellationTokenSource();
			PromotionToken = PromotionSource.Token;
			Figure = new Empty(1, 1);
		}
	}
}
