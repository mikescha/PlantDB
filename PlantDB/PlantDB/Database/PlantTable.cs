using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace PlantDB.Database
{
    public class PlantDatabase
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

    }
}
