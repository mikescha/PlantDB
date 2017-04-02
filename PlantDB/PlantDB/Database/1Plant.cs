﻿using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/* Basic definition of what a Plant is. 
 * First part is the table definition. Potentially more stuff will go below that as needed.
 */
namespace PlantDB.Data
{
    [Table("PlantDB")]
    public partial class Plant : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
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

        private string type;
        [Column("Type")]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
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

        private string sun;
        [Column("Sun")]
        public string Sun
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
                this.inCart = value;
                OnPropertyChanged();
                SetNameInCart(this);
            }
        }

        //This is only set when the FloweringMonth is set. That code is responsible for converting the string from
        //the database into a typed value
       // public FloweringMonths FloweringMonths { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
