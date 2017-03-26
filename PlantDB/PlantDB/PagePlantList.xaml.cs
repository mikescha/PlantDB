using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PlantDB.Data;

namespace PlantDB
{
    public partial class PagePlantList : ContentPage
    {
        public PagePlantList()
        {
            InitializeComponent();
        }

        private void AllPlants_Clicked(object sender, EventArgs e)
        {
            App.PlantData.ShowPlantsByMonth(FloweringMonths.AllMonths);
        }

        private void SomePlants_Clicked(object sender, EventArgs e)
        {
            App.PlantData.ShowPlantsByMonth(FloweringMonths.Feb);
        }

        private void ShowCartPlants_Clicked(object sender, EventArgs e)
        {
            App.PlantData.ShowCartPlants();
        }

        private async void ToggleCartStatus_Clicked(object sender, EventArgs e)
        {
            App.PlantData.ToggleCartStatus((Plant)PlantsListView.SelectedItem);
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void EmptyCart_Clicked(object sender, EventArgs e)
        {
            App.PlantData.EmptyCart();
        }
    }
}
