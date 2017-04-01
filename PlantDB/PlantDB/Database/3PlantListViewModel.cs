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

        private ViewShowing viewShowing; 

        //Constructor. I initialize the list to all months by default
        public PlantListViewModel(string dbPath)
        {
            PlantData = new PlantDatabase(dbPath);
            TargetPlant = new Criteria();
            SetPlantList();
            //GetPlantsByMonth(FloweringMonths.AllMonths);
        }

        //Refreshes the plant list and all other data associated with it
        private void SetPlantList()
        {
            List<Plant> newList;
            if (viewShowing == ViewShowing.Cart)
            {
                newList = PlantData.GetPlantsInCart();
            }
            else
            {
                newList = PlantData.GetAllPlants();
                newList = newList
                            .Where(p => IncludesMonths(TargetPlant.FloweringMonths, p.FloweringMonths))
                            .ToList();
            }



            var sorted = from p in newList
                         orderby p.ScientificName
                         group p by p.Type into plantGroups
                         select new Grouping<string, Plant>(plantGroups.Key, plantGroups);
            
            PlantList = new ObservableCollection<Grouping<string, Plant>>(sorted);

            PlantCount = newList.Count();
        }

        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true 
        private bool IncludesMonths(FloweringMonths wanted, string candidate)
        {
            FloweringMonths[] target = {FloweringMonths.Jan, FloweringMonths.Feb, FloweringMonths.Mar, FloweringMonths.Apr,
                                        FloweringMonths.May, FloweringMonths.Jun, FloweringMonths.Jul, FloweringMonths.Aug,
                                        FloweringMonths.Sep, FloweringMonths.Oct, FloweringMonths.Nov, FloweringMonths.Dec};

            bool result = false;

            foreach (FloweringMonths f in target)
            {
                result = result || (candidate.Contains(floweringMonthDict[f]) && wanted.HasFlag(f));
            }

            return result || wanted.HasFlag(FloweringMonths.AllMonths);
        }

        public Plant GetPlantFromID(int ID)
        {
            return PlantData.GetPlantFromID(ID);
        }

        public void GetPlantsByMonth(FloweringMonths month)
        {
            List<Plant> p;
            if (month == FloweringMonths.AllMonths)
            {
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

            SetPlantList();
        }

        private void GetPlantsByMonth(string month)
        {
            foreach (FloweringMonths f in floweringMonthDict.Keys)
            {
                if (month == floweringMonthDict[f])
                {
                    TargetPlant.FloweringMonths = f;
                    break;
                }
            }
            
            viewShowing = ViewShowing.SomePlants;
            SetPlantList();
        }

        public void GetCartPlants()
        {
            viewShowing = ViewShowing.Cart;
            SetPlantList();
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
                        TargetPlant.ResetCriteria();
                        SetPlantList();
                        //GetPlantsByMonth(FloweringMonths.AllMonths);
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
        #endregion Dictionaries

        #region Flags
        [Flags]
        private enum ViewShowing
        {
            AllPlants = 1, SomePlants = 2, Cart = 4
        }
        #endregion Flags

        #region INPC
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
