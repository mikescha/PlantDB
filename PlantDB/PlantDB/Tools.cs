﻿using System;
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


    #region NeedsHelp
    /* This next block needs some work!!
     * TODO these are basically repeating the same code as in the commands associated with the button, there has got to be a better way to have the button highlight if it is clicked
     * For instance, I could just have a bool for each button, and then update the bool when the button is clicked
     */

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


    // Takes one enum, and returns true if the parameter has the same bit set
    // One use: if the YardSize matches the button ID representing that YardSize then color the button to indicate it is pressed
    public class WaterStringConverter : IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WateringRequirements target = (WateringRequirements)value;
            WateringRequirements button = WateringRequirements.NA;

            switch (parameter)
            {
                case "Dry":
                    button = WateringRequirements.Occasional;
                    break;
                case "Some":
                    button = WateringRequirements.Moderate;
                    break;
                case "Wet":
                    button = WateringRequirements.Regular;
                    break;
            }

            return target.HasFlag(button) ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
    #endregion NeedsHelp

    // Takes a plant and returns an aggregation of all the animals that use it
    public class PlantToAttractsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fullMessage = "";

            if (value != null)
            {
                Plant p = (Plant)value;

                List<string> result = new List<string>();

                if (p.AttractsButterflies.HasFlag(YesNoMaybe.Yes))
                    result.Add("butterflies");

                if (p.AttractsBees.HasFlag(YesNoMaybe.Yes))
                    result.Add("bees");

                if (p.AttractsSongbirds.HasFlag(YesNoMaybe.Yes))
                    result.Add("songbirds");

                if (p.AttractsHummingbirds.HasFlag(YesNoMaybe.Yes))
                    result.Add("hummingbirds");

                switch (result.Count)
                {
                    case 0:
                        fullMessage = "None";
                        break;
                    case 1:
                        fullMessage = char.ToUpper(result[0][0]) + result[0].Substring(1);
                        break;
                    case 2:
                        fullMessage = char.ToUpper(result[0][0]) + result[0].Substring(1) + " and " + result[1];
                        break;
                    default:
                        int i;
                        for (i = 0; i < result.Count - 1; i++)
                        {
                            if (i==0)
                            {
                                fullMessage += char.ToUpper(result[0][0]) + result[0].Substring(1) + ", ";
                            }
                            else
                            {
                                fullMessage += result[i] + ", ";
                            }
                            
                        }
                        fullMessage += "and " + result[i];
                        break;
                }
            }
            
            return fullMessage;
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
    
    // Takes a plant and returns an aggregation of all the animals that use it
    public class CountyIDtoStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO Put the list of counties where this code and the PageLocation code can get it
            //List of counties must be kept in sync with the enum of counties in PlantEnums.cs
            List<string> CountyList = new List<string>()
            {   "All",
                "Alameda", "Alpine", "Amador", "Butte", "Calaveras", "Contra Costa", "Colusa", "Del Norte",
                "El Dorado", "Fresno", "Glenn", "Humboldt", "Imperial", "Inyo", "Kings", "Kern", "Lake",
                "Lassen", "Los Angeles", "Madera", "Mendocino", "Merced", "Mono", "Monterey", "Modoc",
                "Mariposa", "Marin", "Napa", "Nevada", "Orange", "Placer", "Plumas", "Riverside", "Sacramento",
                "Santa Barbara", "San Bernardino", "San Benito", "Santa Clara", "Santa Cruz", "San Diego",
                "San Francisco", "Shasta", "Sierra", "Siskiyou", "San Joaquin", "San Luis Obispo", "San Mateo",
                "Solano", "Sonoma", "Stanislaus", "Sutter", "Tehama", "Trinity", "Tulare", "Tuolumne", "Ventura",
                "Yolo", "Yuba"
            };
            return CountyList[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    //Takes a list of objects, and returns the one at a specified position.
    //Used for databinding the dropdowns
    [ContentProperty("Items")]
    public class ObjectToIndexConverter<T> : IValueConverter
    {
        public IList<T> Items { set; get; }

        public ObjectToIndexConverter()
        {
            Items = new List<T>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null  || Items == null)
                return -1;
            //TODO In my old code, "value is T" would be True if the type of the value was Object(Decimal). In this code, that is returning False. IDKW!!!
            //So, I pulled this out of the expression above and am commenting it out for now. I'm not sure I need it because
            //this was written to make a generic class that would work anywhere and I'm only using it in controlled circumstances.
            //But if it starts crashing for some reason then I'll need to put it back and work through the issue
            //if (value == null  || Items == null || !(value is T) )

            return Items.IndexOf((T)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;

            if (index < 0 || Items == null || index >= Items.Count)
                return null;

            return Items[index];
        }
    }

    // Converts the contents of the Elevation box to a value. Needed because we want to have a special value Any
    // that isn't tied to a particular elevation
    public class ElevationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(value) == -999 ? "Any" : value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;

           return s.ToLower() == "any" ? -999 : value;
        }
    }


    // Takes a YesNoMaybe value, and returns back True if the value is Yes, or False otherwise
    public class YesNoToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (YesNoMaybe)value == YesNoMaybe.Yes ? true : false;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    //Converts a string representing a file to the associated resource file
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            Source = "PlantDB.Images." + Source;
            var imageSource = ImageSource.FromResource(Source);

            return imageSource;
        }
    }


    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            string fileName = "PlantDB.Images." + (string) value + ".png";
            var imageSource = ImageSource.FromResource(fileName);

            return imageSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
