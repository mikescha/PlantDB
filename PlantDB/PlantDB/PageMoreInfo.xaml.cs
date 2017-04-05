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
    public partial class PageMoreInfo : ContentPage
    {
        public PageMoreInfo(string URL)
        {
            InitializeComponent();
            theWeb.Source = new UrlWebViewSource { Url = URL };

        }
    }
}
