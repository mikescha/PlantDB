using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

/*
 * Basic stuff we'll need to work with the plants. This will have enums and other fundamental building blocks
 * that describe plants.
 */
namespace PlantDB.Data
{
    public partial class Plant : INotifyPropertyChanged
    {
        string nameInCart;
        public string NameInCart
        {
            set
            {
                if (nameInCart != value && value != null)
                {
                    nameInCart = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nameInCart;
            }
        }

        // Display names for plants have two versions. 
        //    -- When count is greater then zero, display: "Sunflower (1)"
        //    -- If count is zero, then don't display the parentheses: "Sunflower"
        public void SetNameInCart(Plant p)
        {
            NameInCart = p.PlantName + (p.InCart > 0 ? (" (" + p.InCart.ToString() + ")") : "");
        }
        
        //Used to convert the string representing a month into a typed value. 
        //DEAD CODE, no longer needed but keeping around as it's a good example of how to use dictionary keys
        /*
        public FloweringMonths GetMonthFromString(string month)
        {
            FloweringMonths floweringMonth = FloweringMonths.NA; //Default to NA because that's what we're using for ferns

            if (month != null)
            {
                foreach (FloweringMonths f in floweringMonthDict.Keys)
                {
                    if (month.Contains(floweringMonthDict[f]))
                    {
                        floweringMonth |= f; //These are flags, so if we find a match then use "or" to turn on that flag
                    }
                }
            }
            
            return floweringMonth;
        }*/


        #region Dictionaries
        Dictionary<FloweringMonths, string> floweringMonthDict = new Dictionary<FloweringMonths, string>
        {
            {FloweringMonths.Jan, "Jan" }, {FloweringMonths.Feb, "Feb" }, {FloweringMonths.Mar, "Mar" },
            {FloweringMonths.Apr, "Apr" }, {FloweringMonths.May, "May" }, {FloweringMonths.Jun, "Jun" },
            {FloweringMonths.Jul, "Jul" }, {FloweringMonths.Aug, "Aug" }, {FloweringMonths.Sep, "Sep" },
            {FloweringMonths.Oct, "Oct" }, {FloweringMonths.Nov, "Nov" }, {FloweringMonths.Dec, "Dec" }
           // , {"All", FloweringMonths.AllMonths}
        };
        #endregion Dictionaries


    }
    
}
