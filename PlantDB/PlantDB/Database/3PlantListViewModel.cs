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

        private ObservableCollection<Grouping<string, Plant>> plantList;
        public ObservableCollection<Grouping<string, Plant>> PlantList
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

        private ViewShowing viewShowing; 

        //Constructor. I initialize the list to all months by default
        public PlantListViewModel(string dbPath)
        {
            PlantData = new PlantDatabase(dbPath);
            GetPlantsByMonth(FloweringMonths.AllMonths);
        }

        public Plant GetPlantFromID(int ID)
        {
            return PlantData.GetPlantFromID(ID);
        }

        //Refreshes the plant list and all other data associated with it
        private void SetPlantList(List<Plant> newList)
        {
            //PlantList = new ObservableCollection<Plant>(p);

            var sorted = from p in newList
                         orderby p.ScientificName
                         group p by p.Type into plantGroups
                         select new Grouping<string, Plant>(plantGroups.Key, plantGroups);
            
            PlantList = new ObservableCollection<Grouping<string, Plant>>(sorted);

            PlantCount = newList.Count();
        }

        public void GetPlantsByMonth(FloweringMonths month)
        {
            List<Plant> p;
            if (month == FloweringMonths.AllMonths)
            {
                p = PlantData.GetAllPlants();
                viewShowing = ViewShowing.AllPlants;
            }
            else if (FloweringMonths.AllMonths.HasFlag(month))
            {
                p = PlantData.GetMonthPlants(floweringMonthDict[month]);
                viewShowing = ViewShowing.SomePlants;
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
            viewShowing = ViewShowing.SomePlants;
            SetPlantList(p);
        }

        public void GetCartPlants()
        {
            List<Plant> p;
            p = PlantData.GetPlantsInCart();
            viewShowing = ViewShowing.Cart;
            SetPlantList(p);
        }

        private void UpdateList()
        {
            OnPropertyChanged("PlantList");
            if (viewShowing.HasFlag(ViewShowing.Cart))
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

/* Needed any more?
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
*/
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
                }
                return togglePlantCartStatusCmd;
            }
        }

        //Command for clearing item from cart
        private Command removeItemCmd;
        public ICommand RemoveItemCmd
        {
            get
            {
                if (removeItemCmd == null)
                {
                    removeItemCmd = new Command<Plant>((p) =>
                    {
                        PlantData.ZeroPlantCount(p);
                        UpdateList();
                    });
                }
                return removeItemCmd;
            }
        }

        //Command for adding one plant
        private Command incrementPlantCountCmd;
        public ICommand IncrementPlantCountCmd
        {
            get
            {
                if (incrementPlantCountCmd == null)
                {
                    incrementPlantCountCmd = new Command<Plant>((p) =>
                    {
                        PlantData.IncrementPlantCount(p);
                        UpdateList();
                    });
                }
                return incrementPlantCountCmd;
            }
        }

        //Command for removing one plant
        private Command decrementPlantCountCmd;
        public ICommand DecrementPlantCountCmd
        {
            get
            {
                if (decrementPlantCountCmd == null)
                {
                    decrementPlantCountCmd = new Command<Plant>((p) =>
                    {
                        PlantData.DecrementPlantCount(p);
                        UpdateList();
                    });
                }
                return decrementPlantCountCmd;
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
        private enum ViewShowing
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
