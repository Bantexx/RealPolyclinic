using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RealPolyclinic.HelpMethods
{
    public class myConverter : IMultiValueConverter
    {
        public object Convert(object[] values,Type targettype,object parameter,CultureInfo culture)
        {
            return values.Clone();
        }
        public object[] ConvertBack(object value, Type[] types, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
