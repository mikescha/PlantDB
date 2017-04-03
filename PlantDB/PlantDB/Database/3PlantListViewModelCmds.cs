using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

/* View Model commands
 * 
 * Contains all the basic commands for showing things in the UX
 * 
 */
namespace PlantDB.Data
{
    public partial class PlantListViewModel : INotifyPropertyChanged
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

        //Constructor. I initialize the list to all months by default
        public PlantListViewModel(string dbPath)
        {
            //Load the database
            PlantData = new PlantDatabase(dbPath);

            //Create the default criteria
            TargetPlant = new Criteria();

            //Set defaults for UX
            YardSize = YardSizeTypes.Big;
            YardSun = TargetPlant.Sun;
            
            //Get all the data and init
            SetPlantList();
        }

        public void ShowPlantList()
        {
            viewShowing = ViewShowing.List ;
            SetPlantList();
        }

        public void ShowCartPlants()
        {
            viewShowing = ViewShowing.Cart;
            SetPlantList();
        }

        //Required for scenarios where the data gets updated through the UI, such as when the user types a plant count 
        //into the details field
        public void SavePlant(Plant p)
        {
            PlantData.SavePlant(p);
        }

        /*
         * UI Commands
         */
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
                    });
                }
                return showAllPlantsCmd;
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
                        ShowCartPlants();
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
                    togglePlantCartStatusCmd = new Command<Plant>((p) => { PlantData.ToggleCartStatus(p); });
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
                    removeItemCmd = new Command<Plant>((p) => { PlantData.ZeroPlantCount(p); });
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
                    incrementPlantCountCmd = new Command<Plant>((p) => { PlantData.IncrementPlantCount(p); });
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
                    decrementPlantCountCmd = new Command<Plant>((p) => { PlantData.DecrementPlantCount(p); });
                }
                return decrementPlantCountCmd;
            }
        }

        //Command for setting plant size based on how large the person's yard is
        private Command setYardSizeCmd;
        public ICommand SetYardSizeCmd
        {
            get
            {
                if (setYardSizeCmd == null)
                {
                    setYardSizeCmd = new Command<string>((buttonClicked) => 
                    {
                        switch (buttonClicked)
                        {
                            case "Tiny":
                                TargetPlant.MaxHeight = 3;
                                YardSize = YardSizeTypes.Tiny;
                                break;
                            case "Small":
                                TargetPlant.MaxHeight = 8;
                                YardSize = YardSizeTypes.Small;
                                break;
                            case "Big":
                                TargetPlant.MaxHeight = 1000;
                                YardSize = YardSizeTypes.Big;
                                break;
                        }
                        SetPlantList();

                    });
                }
                return setYardSizeCmd;
            }
        }

        //Command for setting plant size based on how large the person's yard is
        private Command setYardSunCmd;
        public ICommand SetYardSunCmd
        {
            get
            {
                if (setYardSunCmd == null)
                {
                    setYardSunCmd = new Command<string>((buttonClicked) =>
                    {
                        switch (buttonClicked)
                        {
                            case "Full":
                                TargetPlant.Sun ^= SunRequirements.Full;
                                break;
                            case "Part":
                                TargetPlant.Sun ^= SunRequirements.Partial;
                                break;
                            case "Shade":
                                TargetPlant.Sun ^= SunRequirements.Shade;
                                break;
                        }
                        YardSun = TargetPlant.Sun;
                        SetPlantList();

                    });
                }
                return setYardSunCmd;
            }
        }

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
