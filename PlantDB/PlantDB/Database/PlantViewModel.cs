using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlantDB.Data
{
    public class PlantViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plant> plantList;
        public int plantCount;
        public static PlantDatabase PlantData { get; set; }

        //Constructor. I initialize the list to all months by default
        public PlantViewModel(string dbPath)
        {
            PlantData = new PlantDatabase(dbPath);
            PlantList = GetPlantsByMonth(FloweringMonths.AllMonths);
        }

        public void ShowPlantsByMonth(FloweringMonths month)
        {
            PlantList = GetPlantsByMonth(month);
        }

        public void ShowCartPlants()
        {
            PlantList = GetCartPlants();
        }

        private ObservableCollection<Plant> GetPlantsByMonth(FloweringMonths month)
        {
            List<Plant> p;
            if (month == FloweringMonths.AllMonths)
            {
                p = PlantData.GetAllPlants();
            }
            else if (FloweringMonths.AllMonths.HasFlag(month))
            {
                p = PlantData.GetMonthPlants(floweringMonthDict[month]);
            }
            else
            {
                //error, bad month passed in
                p = null;
            }

            PlantCount = p.Count();
            return new ObservableCollection<Plant>(p);
        }

        private ObservableCollection<Plant> GetCartPlants()
        {
            List<Plant> p;
            p = PlantData.GetPlantsInCart();
            PlantCount = p.Count();
            return new ObservableCollection<Plant>(p);
        }

        public bool ToggleCartStatus(Plant p)
        {
            return PlantData.ToggleCartStatus(p);   
        }

        private bool EmptyCart()
        {
            return PlantData.EmptyCart();
        }

        public ObservableCollection<Plant> PlantList
        {
            get
            {
                return plantList;
            }
            set
            {
                if (plantList != value)
                {
                    plantList = value;
                    OnPropertyChanged();
                }
            }
        }

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


        private Command emptyCart;
        public ICommand EmptyCartCmd
        {
            get
            {
                if (emptyCart == null)
                {
                    emptyCart = new Command(() =>
                    {
                        EmptyCart();
                    });
                }
                return emptyCart;
            }
        }


        
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

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INPC
    }
}
