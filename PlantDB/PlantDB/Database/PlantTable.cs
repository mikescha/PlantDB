using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

using SQLite;

namespace PlantDB.Database
{
    public class PlantDatabase : INotifyPropertyChanged
    {
        readonly SQLiteAsyncConnection database;

        public PlantDatabase(string dbPath)
        {
            if (database == null)
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.CreateTableAsync<Plant>().Wait();
            }
            //TODO how do we check if this succeeded or not?
        }

        public Task<List<Plant>> GetAllPlantsAsync()
        {
            return database.Table<Plant>().ToListAsync();
        }

        public Task<List<Plant>> GetSomePlantsAsync()
        {
            return database.Table<Plant>()
                .Where(p => p.FloweringMonths.Contains("Jun"))
                .ToListAsync();
        }

        public Task<List<Plant>> GetPlantsInCartAsync()
        {
            return database.Table<Plant>()
                .Where(p => p.InCart.Equals(1))
                .ToListAsync();
        }

        public Task<int> SavePlantAsync(Plant p)
        {
            return database.UpdateAsync(p);
        }

        /* More Complex Actions */
        public async Task<bool> ToggleCartStatusAsync(Plant p)
        {
            p.InCart = p.InCart == 1 ? 0 : 1;
            return await SavePlantAsync(p) > 0 ? true : false;
        }
        
            

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
