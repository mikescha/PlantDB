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

        private PageLocationViewModel pageLocationViewModel

        {

            get { return BindingContext as PageLocationViewModel; }

        }

        public PageCriteria()
        {    
            InitializeComponent();
            BindingContext = new PageLocationViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Even though this tab doesn't have a plant list, the count is supposed to represent the count of the 
            //current criteria. Calling show plant list will ensure that this happens, in case we came from the 
            //shopping list tab.
            App.PlantData.ShowPlantList();
        }

        private void Button_ShowCritterArea_Clicked(object sender, EventArgs e)
        {
            pageLocationViewModel.ShowCritterArea = !pageLocationViewModel.ShowCritterArea;
        }

        private void Button_ShowSizeArea_Clicked(object sender, EventArgs e)
        {
            pageLocationViewModel.ShowSizeArea = !pageLocationViewModel.ShowSizeArea;
        }

        
    }
}
