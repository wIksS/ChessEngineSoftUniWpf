using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Data
{
	[AddINotifyPropertyChangedInterface]
	public abstract class BasePropertyChanged : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = (sender,e) => { };
	}	
}
