using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PlantDB.Database;

namespace PlantDB
{
    public partial class App : Application
    {
        public static PlantDatabase PlantData { get; set; }

        public App(string dbPath)
        {
            InitializeComponent();

            // Load the resource dictionary where we keep all the style definitions
            if (Application.Current.Resources == null)
            {
                Application.Current.Resources = new ResourceDictionary();
            }

            PlantData = new PlantDatabase(dbPath);

            MainPage = new NavigationPage(new PageTopLevel());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
