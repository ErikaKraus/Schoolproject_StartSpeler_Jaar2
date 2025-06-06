using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Converters
{
public class TableToColorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tableNrEnd = (int)value % 10;
            string hexColor;
            ColorTypeConverter converter = new ColorTypeConverter();

            switch (tableNrEnd)
            {
                case 0: hexColor = "#f5f5dc";
                    break;
                case 1: hexColor = "#007fff";
                    break;
                case 2: hexColor = "#90ee90"; 
                    break;
                case 3: hexColor = "#ff474c";
                    break;
                case 4: hexColor = "#ff80ff";
                    break;
                case 5: hexColor = "#ffffed";
                    break;
                case 6: hexColor = "#007fff";
                    break;
                case 7: hexColor = "#ffc0cb";
                    break;
                case 8: hexColor = "#f08080";
                    break;
                case 9: hexColor = "#f4bb44";
                    break;
                default: hexColor = "#f2a82f"; 
                    break;
            }

            return (Color)converter.ConvertFromInvariantString(hexColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
