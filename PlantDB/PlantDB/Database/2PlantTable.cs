using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using SQLite;
using System.Collections.ObjectModel;

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

        public List<Plant> GetMonthPlants(string month)
        {
            lock(collisionLock)
            {
                return database.Table<Plant>()
                    .Where(p => p.FloweringMonths.Contains(month))
                    .ToList();
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

        public int SavePlant(Plant p)
        {
            lock(collisionLock)
            {
                return database.Update(p);
            }
        }

        /* More Complex Actions */
        public bool ToggleCartStatus(Plant p)
        {
            lock(collisionLock)
            {
                p.InCart = p.InCart > 0 ? 0 : 1;
                return SavePlant(p) > 0 ? true : false;
            }
        }

        public bool ToggleCartStatus(int id)
        {
            lock(collisionLock)
            {
                Plant p = database.Table<Plant>().ElementAt(id);
                return ToggleCartStatus(p);
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

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
