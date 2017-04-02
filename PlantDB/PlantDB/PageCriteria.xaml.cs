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
    public partial class PageCriteria : ContentPage
    {
        public PageCriteria()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //todo: change to just show plants that match the current criteria
            App.PlantData.ShowPlantList();
        }

    }
}
