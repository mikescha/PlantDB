using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/* Basic definition of what a Plant is. 
 * First part is the table definition. Potentially more stuff will go below that as needed.
 */
namespace PlantDB.Data
{
    [Table("Plants")]
    public partial class Plant : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement, Unique]
        [Column("ID")]
        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                this.id = value;
                OnPropertyChanged();
            }
        }

        private string plantName;
        [Column("Plant")]
        public string PlantName
        {
            get
            {
                return plantName;
            }

            set
            {
                this.plantName = value;
                OnPropertyChanged();
            }
        }

        private string plantURL;
        [Column("URL")]
        public string PlantURL
        {
            get
            {
                return plantURL;
            }

            set
            {
                this.plantURL = value;
                OnPropertyChanged();
            }
        }

        private string scientificName;
        [Column("ScientificName")]
        public string ScientificName
        {
            get
            {
                return scientificName;
            }

            set
            {
                this.scientificName = value;
                OnPropertyChanged();
            }
        }

        private PlantTypes plantType;
        [Column("Type")]
        public PlantTypes PlantType
        {
            get
            {
                return plantType;
            }

            set
            {
                this.plantType = value;
                string s;
                switch (value)
                {
                    case PlantTypes.Annual_herb:
                        s = "Annual Herb";
                        break;
                    case PlantTypes.Perennial_herb:
                        s = "Perennial Herb";
                        break;
                    default:
                        s = value.ToString();
                        break;
                }
                PlantTypeString = s;

                OnPropertyChanged();
            }
        }

        private float minHeight;
        [Column("MinHeight")]
        public float MinHeight
        {
            get
            {
                return minHeight;
            }

            set
            {
                this.minHeight = value;
                OnPropertyChanged();
            }
        }

        private float maxHeight;
        [Column("MaxHeight")]
        public float MaxHeight
        {
            get
            {
                return maxHeight;
            }

            set
            {
                this.maxHeight = value;
                OnPropertyChanged();
            }
        }

        private float minWidth;
        [Column("MinWidth")]
        public float MinWidth
        {
            get
            {
                return minWidth;
            }

            set
            {
                this.minWidth = value;
                OnPropertyChanged();
            }
        }

        private float maxWidth;
        [Column("MaxWidth")]
        public float MaxWidth
        {
            get
            {
                return maxWidth;
            }

            set
            {
                this.maxWidth = value;
                OnPropertyChanged();
            }
        }

        private FloweringMonths floweringMonths;
        [Column("FloweringMonths")]
        public FloweringMonths FloweringMonths
        {
            get
            {
                return floweringMonths;
            }

            set
            {
                this.floweringMonths = value;
                //FloweringMonths = GetMonthFromString(value);
                OnPropertyChanged();
            }
        }

        private SunRequirements sun;
        [Column("Sun")]
        public SunRequirements Sun
        {
            get
            {
                return sun;
            }

            set
            {
                this.sun = value;
                OnPropertyChanged();
            }
        }

        private WateringRequirements waterReqs;
        [Column("Watering")]
        public WateringRequirements WaterReqs
        {
            get
            {
                return waterReqs;
            }

            set
            {
                this.waterReqs = value;
                OnPropertyChanged();
            }
        }

        private int minRain;
        [Column("MinRain")]
        public int MinRain
        {
            get
            {
                return minRain;
            }

            set
            {
                this.minRain = value;
                OnPropertyChanged();
            }
        }

        private int maxRain;
        [Column("MaxRain")]
        public int MaxRain
        {
            get
            {
                return maxRain;
            }

            set
            {
                this.maxRain = value;
                OnPropertyChanged();
            }
        }
        
        private FlowerColor flowerColor;
        [Column("FlowerColor")]
        public FlowerColor FlowerColor
        {
            get
            {
                return flowerColor;
            }

            set
            {
                this.flowerColor = value;
                OnPropertyChanged();
            }
        }

        private string visuals;
        [Column("Visuals")]
        public string Visuals
        {
            get
            {
                return visuals;
            }

            set
            {
                this.visuals = value;
                OnPropertyChanged();
            }
        }

        private int minTemp;
        [Column("MinTemp")]
        public int MinTemp
        {
            get
            {
                return minTemp;
            }

            set
            {
                this.minTemp = value;
                OnPropertyChanged();
            }
        }

        private string soil;
        [Column("Soil")]
        public string Soil
        {
            get
            {
                return soil;
            }

            set
            {
                this.soil = value;
                OnPropertyChanged();
            }
        }

        private float minSoilpH;
        [Column("MinSoilpH")]
        public float MinSoilpH
        {
            get
            {
                return minSoilpH;
            }

            set
            {
                this.minSoilpH = value;
                OnPropertyChanged();
            }
        }

        private float maxSoilpH;
        [Column("MaxSoilpH")]
        public float MaxSoilpH
        {
            get
            {
                return maxSoilpH;
            }

            set
            {
                this.maxSoilpH = value;
                OnPropertyChanged();
            }
        }
        
        private Drainages drainage;
        [Column("CNPSDrainage")]
        public Drainages Drainage
        {
            get
            {
                return drainage;
            }

            set
            {
                this.drainage = value;
                OnPropertyChanged();
            }
        }
        
        private YesNoMaybe attractsBees;
        [Column("AttractsBees")]
        public YesNoMaybe AttractsBees
        {
            get
            {
                return attractsBees;
            }

            set
            {
                this.attractsBees = value;
                OnPropertyChanged();
            }
        }
       
        private YesNoMaybe attractsButterflies;
        [Column("AttractsButterflies")]
        public YesNoMaybe AttractsButterflies
        {
            get
            {
                return attractsButterflies;
            }

            set
            {
                this.attractsButterflies = value;
                OnPropertyChanged();
            }
        }

        private YesNoMaybe attractsHummingbirds;
        [Column("AttractsHummingbirds")]
        public YesNoMaybe AttractsHummingbirds
        {
            get
            {
                return attractsHummingbirds;
            }

            set
            {
                this.attractsHummingbirds = value;
                OnPropertyChanged();
            }
        }

        private YesNoMaybe attractsSongbirds;
        [Column("AttractsSongbirds")]
        public YesNoMaybe AttractsSongbirds
        {
            get
            {
                return attractsSongbirds;
            }

            set
            {
                this.attractsSongbirds = value;
                OnPropertyChanged();
            }
        }
        
        private string notes;
        [Column("Notes")]
        public string Notes
        {
            get
            {
                return notes;
            }

            set
            {
                this.notes = value;
                OnPropertyChanged();
            }
        }

        private YesNoMaybe containers;
        [Column("Containers")]
        public YesNoMaybe Containers
        {
            get
            {
                return containers;
            }

            set
            {
                this.containers = value;
                OnPropertyChanged();
            }
        }

        private int minElevation;
        [Column("MinElevation")]
        public int MinElevation
        {
            get
            {
                return minElevation;
            }

            set
            {
                this.minElevation = value;
                OnPropertyChanged();
            }
        }

        private int maxElevation;
        [Column("MaxElevation")]
        public int MaxElevation
        {
            get
            {
                return maxElevation;
            }

            set
            {
                this.maxElevation = value;
                OnPropertyChanged();
            }
        }

        private string countyString;
        [Column("Counties")]
        public string CountyString
        {
            get
            {
                return countyString;
            }

            set
            {
                if (this.countyString != value)
                {
                    this.countyString = value;
                    OnPropertyChanged();
                }
            }
        }

       
        //Note that the only time we set the name in the cart is when the value changes. This works for the intial run because
        //the InCart value is set after the Name is set because of the column order. If this ever got re-ordered so that InCart
        //came before Name, then this code needs to change. Also, this assumes that the user is never allowed to change the 
        //name of the plant.
        private int inCart;
        [Column("InCart")]
        public int InCart
        {
            get
            {
                return inCart;
            }

            set
            {
                if (this.inCart != value)
                {
                    this.inCart = (value >= 0) ? value : 0; //ensure that we don't decrement to negative numbers
                    OnPropertyChanged();
                }
                SetNameInCart(this);

            }
        }

        //This is only set when the Plant Type is set. That code is responsible for converting the typed value from
        //the database into a string. This is used for grouping in the UI; since some of the enum values aren't pretty, 
        //we have to convert them to a string somewhere...
        private string plantTypeString;
        public string PlantTypeString
        {
            get
            {
                return plantTypeString;
            }

            private set
            {
                if (value != null)
                {
                    plantTypeString = value;
                }
                else
                {
                    //TODO why does this happen? I only set this from PlantType but sometimes it's getting set from external code
                    ;
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
    }

    
}
