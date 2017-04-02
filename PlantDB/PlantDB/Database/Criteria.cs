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
        //Fields
        FloweringMonths floweringMonths;
        PlantTypes plantTypes;

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
        }


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

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
