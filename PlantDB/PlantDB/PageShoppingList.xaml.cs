using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageShoppingList : ContentPage
    {
        public PageShoppingList()
        {
            InitializeComponent();
        }

        //When the page loads, we want to update the list to show the current set of plants in the cart
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PlantsListView.SelectedItem = null;
            App.PlantData.ShowCartPlants();
        }
    }
}
