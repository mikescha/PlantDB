﻿using System;
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
            App.PlantData.GetPlantsByMonth(FloweringMonths.AllMonths);
            PlantsListView.SelectedItem = null;
        }
    }
}
