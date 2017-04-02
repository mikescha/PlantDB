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
        private ViewShowing viewShowing;

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


        #region Flags
        [Flags]
        private enum ViewShowing
        {
            List = 1, Cart = 2
        }
        #endregion Flags
    }
}
