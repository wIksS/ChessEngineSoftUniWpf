using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Convertors
{
    public class CpConvertor : System.Windows.Data.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int cp = (int)values[0];
            double AH = (double)values[1];
            double res = (double)(AH/2 - ((double)AH / 1600.0)* Math.Min(Math.Max(cp, -800), 800));
            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
