using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using SQLite;

namespace PlantDB.Data
{
    public class PlantDatabase : INotifyPropertyChanged
    {
        readonly SQLiteConnection database;

        public PlantDatabase(string dbPath)
        {
            if (database == null)
            {
                database = new SQLiteConnection(dbPath);
                database.CreateTable<Plant>();
            }
            //TODO how do we check if this succeeded or not?
        }

        public List<Plant> GetAllPlants()
        {
            return database.Table<Plant>().ToList();
        }

        public List<Plant> GetMonthPlants(string month)
        {
            return database.Table<Plant>()
                .Where(p => p.FloweringMonths.Contains(month))
                .ToList();
        }

        public List<Plant> GetPlantsInCart()
        {
            return database.Table<Plant>()
                .Where(p => p.InCart > 0 )
                .ToList();
        }

        public int SavePlant(Plant p)
        {
            return database.Update(p);
        }

        /* More Complex Actions */
        public bool ToggleCartStatus(Plant p)
        {
            p.InCart = p.InCart > 0 ? 0 : 1;
            return SavePlant(p) > 0 ? true : false;
        }

        public bool EmptyCart()
        {
            // Get the list of plants that are currently in the cart
            List<Plant> cart = GetPlantsInCart();

            //Walk the list, clear each item, and then save it back
            foreach (Plant p in cart)
            {
                p.InCart = 0;
                SavePlant(p);
            }
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
