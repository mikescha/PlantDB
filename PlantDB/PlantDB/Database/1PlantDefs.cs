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

    }

    [Flags]
    public enum FloweringMonths
    {
        Unassigned = 1, Unknown = 2, NotApplicable = 4,
        Jan = 8, Feb = 16, Mar = 32, Apr = 64, May = 128, Jun = 256,
        Jul = 512, Aug = 1024, Sep = 2048, Oct = 4096, Nov = 8192, Dec = 16384,
        AllMonths = (Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec)
    }

}
