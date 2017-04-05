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
        public int AnyElevation = -999;
        public int AnyTemp = 999;


        //Constructor -- set all the defaults to whatever we want the starting state to be
        public Criteria()
        {
            ResetCriteria();
            County = Counties.All;
            Elevation = AnyElevation;
            MinTemp = AnyTemp;
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
            WaterReqs = WateringRequirements.NA;
        }

        #region fields
        private WateringRequirements waterReqs;
        public WateringRequirements WaterReqs
        {
            set
            {
                if (waterReqs != value)
                {
                    waterReqs = value;
                    OnPropertyChanged();
                }
            }
            get { return waterReqs; }
        }

        private FloweringMonths floweringMonths;
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

        private PlantTypes plantTypes;
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

        private SunRequirements sun;
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

        private float maxHeight;
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
        private Counties county;
        public Counties County
        {
            set
            {
                if (county != value)
                {
                    county = value;
                    OnPropertyChanged();
                }
            }
            get { return county; }
        }

        private int elevation;
        public int Elevation
        {
            set
            {
                if (elevation != value)
                {
                    elevation = value;
                    OnPropertyChanged();
                }
            }
            get { return elevation; }
        }

        private int minTemp;
        public int MinTemp
        {
            set
            {
                if (minTemp != value)
                {
                    minTemp = value;
                    OnPropertyChanged();
                }
            }
            get { return minTemp; }
        }
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
