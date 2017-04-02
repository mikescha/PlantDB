using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using SQLite;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PlantDB.Data
{
    public class PlantDatabase : INotifyPropertyChanged
    {
        readonly SQLiteConnection database;
        private static object collisionLock = new object();

        public PlantDatabase(string dbPath)
        {
            if (database == null)
            {
                //TODO how do we check if this succeeded or not?
                database = new SQLiteConnection(dbPath);
                database.CreateTable<Plant>();
            }
        }

        public List<Plant> GetAllPlants()
        {
            lock(collisionLock)
            {
                return database.Table<Plant>().ToList();
            }
        }

        public List<Plant> GetPlantsInCart()
        {
            lock(collisionLock)
            {
                return database.Table<Plant>()
                    .Where(p => p.InCart > 0)
                    .ToList();
            }

        }

        //Writes changes to the database
        //Returns number of rows updated. This should always be just one, since a plant is only in the database once
        public int SavePlant(Plant p)
        {
            lock(collisionLock)
            {
                int result = database.Update(p);

                if (result == 0)
                {
                    //!!TODO need to generate an exception, as this should never happen
                }

                return result;
            }
        }

        /*********** More Complex Actions ***********/

        //If plant count was zero, then makes it one, and vice versa, then saves changes to DB
        //Returns true if it succeeded. 

        public bool ToggleCartStatus(Plant p)
        {
            lock(collisionLock)
            {
                bool result;
                if (p!= null)
                {
                    p.InCart = p.InCart > 0 ? 0 : 1;
                    result = SavePlant(p) > 0 ? true : false;
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }

        //If plant count was zero, then makes it one, and vice versa, then saves changes to DB
        //Returns true if it succeeded.
        public bool ZeroPlantCount(Plant p)
        {
            lock (collisionLock)
            {
                p.InCart = 0;
                return SavePlant(p) > 0 ? true : false;
            }
        }

        //If plant count was zero, then makes it one, and vice versa, then saves changes to DB
        //Returns true if it succeeded.
        public bool IncrementPlantCount(Plant p)
        {
            lock (collisionLock)
            {
                p.InCart++;
                return SavePlant(p) > 0 ? true : false;
            }
        }

        //If plant count was zero, then makes it one, and vice versa, then saves changes to DB
        //Returns true if it succeeded.
        public bool DecrementPlantCount(Plant p)
        {
            lock (collisionLock)
            {
                p.InCart--;

                if (p.InCart < 0)
                {
                    //TODO need to generate an exception, as this should never happen
                }
                return SavePlant(p) > 0 ? true : false;
            }
        }


        public void EmptyCart()
        {
            lock(collisionLock)
            {
                // Get the list of plants that are currently in the cart
                List<Plant> cart = GetPlantsInCart();

                //Walk the list, clear each item, and then save it back
                foreach (Plant p in cart)
                {
                    p.InCart = 0;
                    SavePlant(p);
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
