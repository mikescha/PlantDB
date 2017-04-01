using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.ObjectModel;
using PlantDB.Data;

namespace PlantDB
{
    // Takes an int, and returns back PosText if the parameter was True, or ZeroText otherwise. 
    // One use: If the shopping cart count is zero, then format the list item a different color
    public class IntToStringConverter : IValueConverter
    {
        public string PosText { set; get; }
        public string ZeroText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value>0 ? PosText : ZeroText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    public class PlantIDToStringConverter : IValueConverter
    {
        public string RemoveText { set; get; }
        public string AddText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return (int)value>0 ? RemoveText : AddText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
