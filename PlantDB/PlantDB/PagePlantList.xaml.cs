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

        // When the "More Info" link is tapped, then open a new pop-over window and load the URL of the selected plant into this
        //TODO When the more info window closes, the PlantList page gets an "OnAppearing" message, which causes the selection to be cleared. How else can i do this?
        //For instance, should I just render the webview in my own scrollview, instead of creating a whole new window?
        async void OnMoreInfoTapped(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Plant thePlant = (Plant)label.BindingContext;
            if (thePlant.PlantURL != null)
            {
                await Navigation.PushAsync(new PageMoreInfo(thePlant.PlantURL));
            }
            else
            {
                //TODO: what to do if there is no URL for the plant
            }
        }

        //do anything?
        private void PlantsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        //Opens new window with the image slideshow
        async private void OnPlantImageTapped(object sender, EventArgs e)
        {
            Image image = (Image)sender;
            Plant thePlant = (Plant)image.BindingContext;
            if (thePlant.WebImages != null)
            {
                await Navigation.PushAsync(new PagePlantImages(thePlant.WebImages));
            }
        }
    }
}
