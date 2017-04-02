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
        //Constructor -- set all the defaults to whatever we want the starting state to be, as the criteria gets
        //applied every time the list is drawn
        public Criteria()
        {
            ResetCriteria();
        }

        public void ResetCriteria()
        {
            FloweringMonths = FloweringMonths.AllMonths;
            PlantTypes = PlantTypes.AllPlantTypes;
            Sun = SunRequirements.AllSunTypes;

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
