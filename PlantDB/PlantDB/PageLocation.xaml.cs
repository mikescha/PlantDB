using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Geolocator; // From https://github.com/jamesmontemagno/GeolocatorPlugin/blob/master/README.md
                         // Also https://blog.xamarin.com/geolocation-for-ios-android-and-windows-made-easy/
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Net.Http;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLocation : ContentPage
    {
        public PageLocation()
        {
            InitializeComponent();
            GetLocation();
        }

        async void GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            Plugin.Geolocator.Abstractions.Position position = null;

            try
            {
                position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            catch (Exception e)
            {
                position = null;
                //TODO Turn off the spinner, show the error message label

            }
            
            labelStatus.Text = "Your location:";

            if (position != null)
            {
                App.PlantData.TargetPlant.Lat = position.Latitude;
                App.PlantData.TargetPlant.Lng = position.Longitude;

                ReverseGeocode();
            }

            //SetUserCounty();

            
            if (App.PlantData.TargetPlant.UserCounty != "")
            {
                labelResult.Text = "If this is not the location you want, use the Customize controls to choose a different county.";
            }
            else if (position != null)
            {
                labelResult.Text = "You are outside the area supported by this app. Use the Customize controls to choose a county within range.";
            }
            else
            {
                labelResult.Text = "Unable to get your location. Please use the Customize controls to pick your county.";
            }
            labelResult.IsVisible = true;

        }

        private class AddressElement
        {
            public string long_name;
            public string short_name;
            public string type;
        }

        private async void ReverseGeocode()
        {
            string GoogleKey = "AIzaSyCTQFfvVgPT7Czy8ddpbAzVo1QB2y894ws";

            double lat = App.PlantData.TargetPlant.Lat;
            double lng = App.PlantData.TargetPlant.Lng;

            try
            {
                var httpClient = new HttpClient();
                var requestString = string.Format(@"https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&key={2}",
                                                    lat.ToString(), lng.ToString(), GoogleKey);
                var responseString = await httpClient.GetStringAsync(requestString);

                string[] items = await ParseMapResponse(responseString);

                labelCityState.Text = items[0] + ", " + items[1];
                labelCounty.Text = items[2];
                App.PlantData.TargetPlant.UserCounty = items[2];
            }
            catch
            {
                //Error
                ;
            }
            
        }

        private async Task<string[]> ParseMapResponse(string response)
        {
            return await Task.Run(() =>
            {
                var xdoc = XDocument.Parse(response);
                var result = xdoc.Root.Descendants("address_component").Select(x => new AddressElement
                {
                    long_name = x.Element("long_name").Value,
                    short_name = x.Element("short_name").Value,
                    type = x.Element("type").Value
                });

                //In the Google Maps API, City is type=="political", Count is type=="administrative_area_level_2", 
                //State is type=="administrative_area_level_1"
                //For some reason there are duplicate results in the response, so select just the first one
                IEnumerable<AddressElement> element = result.Where(p => p.type == "political");
                string city = element.Any() ? element.First().long_name : "";

                //Get the state
                element = result.Where(p => p.type == "administrative_area_level_1");
                string state = element.Any() ? element.First().long_name : "";

                //Get the county and write it to both the UI and to the data structure
                element = result.Where(p => p.type == "administrative_area_level_2");
                string county = element.Any() ? element.First().long_name : "";
                string[] s = { city, state, county };
                return s;
            });
        }
    }
}
