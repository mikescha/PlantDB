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
    public class PlantListViewModel : INotifyPropertyChanged
    {
        public static PlantDatabase PlantData { get; set; }

        private ObservableCollection<Plant> plantList;
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

        private view viewShowing; 

        //Constructor. I initialize the list to all months by default
        public PlantListViewModel(string dbPath)
        {
            PlantData = new PlantDatabase(dbPath);
            GetPlantsByMonth(FloweringMonths.AllMonths);
        }

        //Refreshes the plant list and all other data associated with it
        private void SetPlantList(List<Plant> p)
        {
            PlantList = new ObservableCollection<Plant>(p);
            PlantCount = p.Count();
        }

        private void GetPlantsByMonth(FloweringMonths month)
        {
            List<Plant> p;
            if (month == FloweringMonths.AllMonths)
            {
                p = PlantData.GetAllPlants();
                viewShowing = view.AllPlants;
            }
            else if (FloweringMonths.AllMonths.HasFlag(month))
            {
                p = PlantData.GetMonthPlants(floweringMonthDict[month]);
                viewShowing = view.SomePlants;
            }
            else
            {
                //error, bad month passed in
                p = null;
            }

            SetPlantList(p);
        }

        private void GetPlantsByMonth(string month)
        {
            List<Plant> p = PlantData.GetMonthPlants(month);
            viewShowing = view.SomePlants;
            SetPlantList(p);
        }

        private void GetCartPlants()
        {
            List<Plant> p;
            p = PlantData.GetPlantsInCart();
            viewShowing = view.Cart;
            SetPlantList(p);
        }

        private void UpdateList()
        {
            if (viewShowing.HasFlag(view.Cart))
            {
                GetCartPlants();
            }
        }
 
        private Command showAllPlantsCmd;
        public ICommand ShowAllPlantsCmd
        {
            get
            {
                if (showAllPlantsCmd == null)
                {
                    showAllPlantsCmd = new Command(() => 
                    {
                        GetPlantsByMonth(FloweringMonths.AllMonths);
                    });
                }
                return showAllPlantsCmd;
            }
        }

        private Command showSomePlantsCmd;
        public ICommand ShowSomePlantsCmd
        {
            get
            {
                if (showSomePlantsCmd == null)
                {
                    showSomePlantsCmd = new Command<string>((month) => { GetPlantsByMonth(month); });
                }
                return showSomePlantsCmd;
            }
        }

        private Command showCartPlantsCmd;
        public ICommand ShowCartPlantsCmd
        {
            get
            {
                if (showCartPlantsCmd == null)
                {
                    showCartPlantsCmd = new Command(() => { GetCartPlants(); });
                }
                return showCartPlantsCmd;
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
                        PlantData.EmptyCart();
                        UpdateList();
                    });
                }
                return emptyCart;
            }
        }

        private Command togglePlantCartStatusCmd;
        public ICommand TogglePlantCartStatusCmd
        {
            get
            {
                if (togglePlantCartStatusCmd == null)
                {
                    togglePlantCartStatusCmd = new Command<Plant>((p) =>
                    {
                        PlantData.ToggleCartStatus(p);
                        UpdateList();
                    });
                    /*togglePlantCartStatusCmd = new Command<int>((id) =>
                    {
                        PlantData.ToggleCartStatus(id);
                       UpdateList();
                   });*/
                }
                return togglePlantCartStatusCmd;
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

        [Flags]
        private enum view
        {
            AllPlants = 1, SomePlants = 2, Cart = 4
        }
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
