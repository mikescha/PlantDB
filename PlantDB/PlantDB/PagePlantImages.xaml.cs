using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePlantImages : ContentPage
    {
        public PagePlantImages(string imageList)
        {
            InitializeComponent();
            string[] imageStrings = imageList.Split('\n');
            ObservableCollection<string> theImages = new ObservableCollection<string>();
            foreach (string s in imageStrings)
            {
                theImages.Add(s);
            }
            ImageListView.ItemsSource = theImages;
        }
    }
}
