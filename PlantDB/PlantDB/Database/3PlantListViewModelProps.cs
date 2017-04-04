using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

/* View Model properties
 * 
 * Contains all the properties and definitions
 * 
 */
namespace PlantDB.Data
{
    public partial class PlantListViewModel : INotifyPropertyChanged
    {
        public ViewShowing viewShowing;

        private int plantCount;
        public int PlantCount
        {
            get
            {
                return plantCount;
            }
            private set
            {
                if (plantCount != value)
                {
                    plantCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private Criteria targetPlant;
        public Criteria TargetPlant
        {
            get
            {
                return targetPlant;
            }
            private set
            {
                if (targetPlant != value)
                {
                    targetPlant = value;
                    OnPropertyChanged();
                }
            }
        }

        private YardSizeTypes yardSize;
        public YardSizeTypes YardSize
        {
            get
            {
                return yardSize;
            }
            private set
            {
                if (yardSize != value)
                {
                    yardSize = value;
                    OnPropertyChanged();
                }
            }
        }

        private SunRequirements yardSun;
        public SunRequirements YardSun
        {
            get
            {
                return yardSun;
            }
            private set
            {
                if (yardSun != value)
                {
                    yardSun = value;
                    OnPropertyChanged();
                }
            }
        }

    }

    #region Flags
    [Flags]
    //Note that these are used in the UI, so don't change the numbers or order without doublechecking all the places they're used!
    public enum YardSizeTypes
    {
        NA = 0, Tiny = 1, Small = 2, Big = 4
    }

    public enum ViewShowing
    {
        List = 1, Cart = 2
    }
    #endregion Flags

}

