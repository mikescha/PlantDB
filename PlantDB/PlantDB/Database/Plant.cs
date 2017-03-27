using SQLite;
using System.ComponentModel;

/* Basic definition of what a Plant is. 
 * First part is the table definition. Potentially more stuff will go below that as needed.
 */
namespace PlantDB.Data  
{
    [Table("PlantDB")]

    public partial class Plant : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Plant")]
        public string PlantName { get; set; }

        [Column("ScientificName")]
        public string ScientificName { get; set; }

        [Column("Type")]
        public string Type { get; set; }

        [Column("MinHeight")]
        public float MinHeight { get; set; }

        [Column("MaxHeight")]
        public float MaxHeight { get; set; }

        [Column("FloweringMonths")]
        public string FloweringMonths { get; set; }

        [Column("Sun")]
        public string Sun { get; set; }

        [Column("InCart")]
        public int InCart { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

    }
}
