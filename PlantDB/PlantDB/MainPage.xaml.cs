using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using PlantDB.Database;

namespace PlantDB
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Plant> plants { get; set; }

        public MainPage()
        {
            InitPlantList();
            InitializeComponent();
            myLabel.Text = "Your plants, sir:";
        }

        public async void InitPlantList()
        {
            List<Plant> p = await App.PlantData.GetAllPlantsAsync();
            plants = new ObservableCollection<Plant>(p);
            PlantsListView.ItemsSource = plants;
        }

        private async void AllPlants_Clicked(object sender, EventArgs e)
        {
            List<Plant> p = await App.PlantData.GetAllPlantsAsync();
            plants = new ObservableCollection<Plant>(p);
            myLabel.Text = "Now showing all plants";
            PlantsListView.ItemsSource = plants;
        }

        private async void SomePlants_Clicked(object sender, EventArgs e)
        {
            List<Plant> p = await App.PlantData.GetSomePlantsAsync();
            plants = new ObservableCollection<Plant>(p);
            myLabel.Text = "Now showing --some-- plants";
            PlantsListView.ItemsSource = plants;

        }

        private async void ToggleCartPlants_Clicked(object sender, EventArgs e)
        {
            List<Plant> p = await App.PlantData.GetPlantsInCartAsync();
            plants = new ObservableCollection<Plant>(p);
            myLabel.Text = "Now showing plants in shopping cart";
            PlantsListView.ItemsSource = plants;
        }

        private async void ToggleCartStatus_Clicked(object sender, EventArgs e)
        {
            Plant p = (Plant) PlantsListView.SelectedItem;
            p.InCart = p.InCart == 1 ? 0 : 1;
            await App.PlantData.SavePlantAsync(p);
        }

    }
}
