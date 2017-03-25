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
            myLabel.Text = "Your plants, sir:";
        }

        private async void AllPlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetAllPlantsAsync();
             
            myLabel.Text = "Now showing all plants";

        }

        private async void SomePlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetSomePlantsAsync();

            myLabel.Text = "Now showing --some-- plants";

        }

        private async void ToggleCartPlants_Clicked(object sender, EventArgs e)
        {
            PlantsListView.ItemsSource = await App.PlantData.GetPlantsInCartAsync();

            myLabel.Text = "Now showing plants in shopping cart";

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

    }
}
