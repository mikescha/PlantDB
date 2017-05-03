using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePlantImages : ContentPage
    {
        ObservableCollection<string> TheImageURLs { get; set; }

        public PagePlantImages(string imageList, string plantName)
        {
            InitializeComponent();
            string[] imageStrings = imageList.Split('\n');
            TheImageURLs = new ObservableCollection<string>();
            foreach (string s in imageStrings)
            {
                TheImageURLs.Add(s);
            }
            PlantName.Text = plantName;
            ImageCarousel.ItemsSource = TheImageURLs;
        }
    }
}
