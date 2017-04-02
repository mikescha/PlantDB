using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PlantDB.Data;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePlantList : ContentPage
    {
        public PagePlantList()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PlantsListView.SelectedItem = null;
            App.PlantData.ShowPlantList();
        }

        //Occurs when the user changes the plant count by typing in a new value
        //This gets changed in the data structure, but needs to be written to the database to be persisted
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Plant p = (Plant) PlantsListView.SelectedItem;
            App.PlantData.SavePlant(p);
        }
    }
}
