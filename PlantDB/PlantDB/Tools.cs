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
    //Used for doing the grouping in the listview of plants
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
    }

    [ContentProperty("Source")]
    public class GetImageResource : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) return null;
            return ImageSource.FromResource(Source);
        }
    }

    // Takes a SunType, and returns back a pretty string
    public class SunToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s;

            switch ((SunRequirements) value)
            {
                case SunRequirements.AllSunTypes:
                    s = "Any amount of sun";
                    break;
                case SunRequirements.Partial:
                    s = "Partial sun";
                    break;
                case SunRequirements.Shade:
                    s = "Most or full shade";
                    break;
                case SunRequirements.Full:
                    s = "Full or mostly sun";
                    break;
                case SunRequirements.Partial | SunRequirements.Full:
                    s = "Full sun to partial sun";
                    break;
                case SunRequirements.Partial | SunRequirements.Shade:
                    s = "Partial sun to full shade";
                    break;

                default:
                    s = value.ToString();
                    break;
            }

            return s;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

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

    //Takes the count of plants in the shopping cart and returns a string appropriate for the context menu
    public class PlantCountToStringConverter : IValueConverter
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

    // Takes a list, and if the list has at least one element then it returns False. If the list is null or has zero elements, it returns True. 
    // One use: If the list of plants is empty, then set the visible state of the "No matching plants" label to True.
    public class ListToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            return ((IList)value).Count == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes a list, and if the list has at least one element then it returns TRUE. If the list is null or has zero elements, it returns True. 
    // One use: If the list of plants is empty, then set the visible state of the Detail panel label to False.
    public class ListToBoolConverterOpposite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((IList)value).Count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes an object, and if the object exists then it returns False. If the object is null, it returns True. 
    // One use: If nothing is selected in the list of plants, then set the visible state of the "Select an plant to see detail" label to True.
    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes an object, and if the object exists then it returns True. If the object is null, it returns False. 
    // One use: If nothing is selected in the list of plants, then set the visible state of the Show Detail panel to False.
    public class ObjectToBoolConverterOpposite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes a number, and if the number is positive then it returns True. If the number is negative, it returns False. 
    public class NumberToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((int)value > 0) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes a number, and if the number is positive then it returns False. If the number is negative, it returns True. 
    public class NumberToBoolConverterOpposite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((double)value == 0) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes a boolean, and returns back TrueText if the parameter was True, or FalseText otherwise. 
    // One use: If the shopping cart state is False, then show the text, "Add to Cart"
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    // Takes one enum, and returns true if the parameter has the same bit set
    // One use: if the YardSize matches the button ID representing that YardSize then color the button to indicate it is pressed
    public class YardSizeStringConverter : IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            YardSizeTypes target = (YardSizeTypes)value;
            YardSizeTypes button = YardSizeTypes.NA;

            switch(parameter){
                case "Tiny":
                    button = YardSizeTypes.Tiny;
                    break;
                case "Small":
                    button = YardSizeTypes.Small;
                    break;
                case "Big":
                    button = YardSizeTypes.Big;
                    break;
            }

            return target.HasFlag(button) ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    // Takes one enum, and returns true if the parameter has the same bit set
    // One use: if the YardSize matches the button ID representing that YardSize then color the button to indicate it is pressed
    public class SunStringConverter : IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SunRequirements target = (SunRequirements)value;
            SunRequirements button = SunRequirements.NA;

            switch (parameter)
            {
                case "Full":
                    button = SunRequirements.Full;
                    break;
                case "Part":
                    button = SunRequirements.Partial;
                    break;
                case "Shade":
                    button = SunRequirements.Shade;
                    break;
            }

            return target.HasFlag(button) ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }


    // Takes a boolean, and returns back the opposite. 
    // One use: If the error message is visible then stop the spinner.
    public class BoolNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
