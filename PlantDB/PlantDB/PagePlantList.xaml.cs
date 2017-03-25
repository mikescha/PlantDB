using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PlantDB.Database;

namespace PlantDB
{
    public partial class PagePlantList : ContentPage
    {
        public PagePlantList()
        {
            InitializeComponent();
        }

        private async void AllPlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetAllPlantsAsync();
        }

        private async void SomePlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetSomePlantsAsync();
        }

        private async void ToggleCartPlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetPlantsInCartAsync();
        }

        private async void ToggleCartStatus_Clicked(object sender, EventArgs e)
        {
            await App.PlantData.ToggleCartStatusAsync((Plant)PlantsListView.SelectedItem);
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            PlantsListView.ItemsSource = await App.PlantData.GetAllPlantsAsync();
        }

        private async void EmptyCart_Clicked(object sender, EventArgs e)
        {
            await App.PlantData.EmptyCart();

        }
    }
}
