using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.Drawing;
using ImageService.Logging.Model;
using System.Diagnostics;
using System.Windows;

namespace GUI.Views.UserControls
{
    class TypeToBrushConverter : IValueConverter
    {
        IDictionary<MessageTypeEnum, string> typeToBrush =
           new Dictionary<MessageTypeEnum, string>()
           {
               { MessageTypeEnum.INFO,"LightGreen"},
               { MessageTypeEnum.WARNING,"Yellow" },
               { MessageTypeEnum.FAIL,"Red" }

           };
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value.ToString();
            MessageTypeEnum myStatus;
            Enum.TryParse(val, out myStatus);
            return typeToBrush[myStatus];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
