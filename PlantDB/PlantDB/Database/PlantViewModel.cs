using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PlantDB.Data
{
    public class PlantViewModel : INotifyPropertyChanged
    {
        public NotifyTask<ObservableCollection<Plant>> plantList;
        public int plantCount;
        public static PlantDatabase PlantData { get; set; }

        //Constructor. I initialize the list to all months by default
        public PlantViewModel(string dbPath)
        {
            PlantData = new PlantDatabase(dbPath);
            PlantList = NotifyTask.Create(GetPlantsAsync(FloweringMonths.AllMonths));
        }

        public void ShowPlantsByMonth(FloweringMonths month)
        {
            PlantList = NotifyTask.Create(GetPlantsAsync(month));
        }

        public void ShowCartPlants()
        {
            PlantList = NotifyTask.Create(GetCartPlantsAsync());
        }

        public void ToggleCartStatus(Plant p)
        {
            NotifyTask.Create(ToggleCartStatusAsync(p));
        }

        public void EmptyCart()
        {
            NotifyTask.Create(EmptyCartAsync());
        }

        private async Task<ObservableCollection<Plant>> GetPlantsAsync(FloweringMonths month)
        {
            List<Plant> p;
            if (month == FloweringMonths.AllMonths)
            {
                p = await PlantData.GetAllPlantsAsync();
            }
            else if (FloweringMonths.AllMonths.HasFlag(month))
            {
                p = await PlantData.GetMonthPlantsAsync(floweringMonthDict[month]);
            }
            else
            {
                //error, bad month passed in
                p = null;
            }

            PlantCount = p.Count();
            return new ObservableCollection<Plant>(p);
        }

        private async Task<ObservableCollection<Plant>> GetCartPlantsAsync()
        {
            List<Plant> p;
            p = await PlantData.GetPlantsInCartAsync();
            PlantCount = p.Count();
            return new ObservableCollection<Plant>(p);
        }

        private async Task<bool> ToggleCartStatusAsync(Plant p)
        {
            return await PlantData.ToggleCartStatusAsync(p);   
        }

        private async Task<bool> EmptyCartAsync()
        {
            return await PlantData.EmptyCart();
        }

        public NotifyTask<ObservableCollection<Plant>> PlantList
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
