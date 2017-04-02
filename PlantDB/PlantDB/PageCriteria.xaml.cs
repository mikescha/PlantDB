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

            //Even though this tab doesn't have a plant list, the count is supposed to represent the count of the 
            //current criteria. Calling show plant list will ensure that this happens, in case we came from the 
            //shopping list tab.
            App.PlantData.ShowPlantList();
        }

    }
}
