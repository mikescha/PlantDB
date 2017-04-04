using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PlantDB.Data
{
    public class Criteria : INotifyPropertyChanged
    {
        //Constructor -- set all the defaults to whatever we want the starting state to be
        public Criteria()
        {
            ResetCriteria();
            targetCounty = Counties.None;
        }

        //Called at launch time, but also any time the user picks, "Show all plants". This should only reset the
        //criteria that are location independent! i.e. don't reset altitude or location, as when the user clears their
        //filters they should still only see plants valid for their location. 
        public void ResetCriteria()
        {
            FloweringMonths = FloweringMonths.AllMonths;
            PlantTypes = PlantTypes.AllPlantTypes;
            Sun = SunRequirements.AllSunTypes;
            MaxHeight = 1000;

        }

        #region fields
        FloweringMonths floweringMonths;
        public FloweringMonths FloweringMonths
        {
            set
            {
                if (floweringMonths != value)
                {
                    floweringMonths = value;
                    OnPropertyChanged();
                }
            }
            get { return floweringMonths; }
        }

        PlantTypes plantTypes;
        public PlantTypes PlantTypes
        {
            set
            {
                if (plantTypes != value)
                {
                    plantTypes = value;
                    OnPropertyChanged();
                }
            }
            get { return plantTypes; }
        }

        SunRequirements sun;
        public SunRequirements Sun
        {
            set
            {
                if (sun != value)
                {
                    sun = value;
                    OnPropertyChanged();
                }
            }
            get { return sun; }
        }

        float maxHeight;
        public float MaxHeight
        {
            set
            {
                if (maxHeight != value)
                {
                    maxHeight = value;
                    OnPropertyChanged();
                }
            }
            get { return maxHeight; }
        }

        //counties that the user wants
        public Counties targetCounty;

        #endregion fields

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
