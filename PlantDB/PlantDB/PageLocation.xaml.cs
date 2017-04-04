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
using System.Collections.ObjectModel;

namespace PlantDB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLocation : ContentPage
    {
        #region constants and fields
        //List of counties must be kept in sync with the enum of counties in PlantEnums.cs
        public static List<string> CountyList = new List<string>()
            {   "All",
                "Alameda", "Alpine", "Amador", "Butte", "Calaveras", "Contra Costa", "Colusa", "Del Norte",
                "El Dorado", "Fresno", "Glenn", "Humboldt", "Imperial", "Inyo", "Kings", "Kern", "Lake",
                "Lassen", "Los Angeles", "Madera", "Mendocino", "Merced", "Mono", "Monterey", "Modoc",
                "Mariposa", "Marin", "Napa", "Nevada", "Orange", "Placer", "Plumas", "Riverside", "Sacramento",
                "Santa Barbara", "San Bernardino", "San Benito", "Santa Clara", "Santa Cruz", "San Diego",
                "San Francisco", "Shasta", "Sierra", "Siskiyou", "San Joaquin", "San Luis Obispo", "San Mateo",
                "Solano", "Sonoma", "Stanislaus", "Sutter", "Tehama", "Trinity", "Tulare", "Tuolumne", "Ventura",
                "Yolo", "Yuba"
            };
        //Coordinates we get from the GPS
        private double lat = 0;
        private double lng = 0;
        
        //Stores the county that we get from the Geolocation/Reverse Geocoding process. This is then converted 
        //to the county the user wants, which gets stored in the view model
        private string GPSCounty = "";
        #endregion constants and fields

        public PageLocation()
        {
            InitializeComponent();
            
            //Necessary so that the label can show which county is selected. Can't figure out how to do this in XAML :-(
            BindingContext = App.PlantData.TargetPlant;

            countyListView.ItemsSource = searchCounty;
            GetLocation();
        }

        //Gets the user's current location, stores it in the ViewModel, and updates the UI to show the user
        async Task GetLocation()
        {
            Plugin.Geolocator.Abstractions.Position position;

            position = await GetGPSCoordinates();
            if (position != null)
            {
                lat = position.Latitude;
                lng = position.Longitude;

                await ReverseGeocode();

                SetUserCounty();
            }
            ReportResults();
        }
        
        //used in ParseMapResponse only
        private class AddressElement
        {
            public string long_name;
            public string short_name;
            public string type;
        }

        //call the device's location service to get the GPS coordinates
        private async Task<Plugin.Geolocator.Abstractions.Position> GetGPSCoordinates()
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
            }
            return position;
        }

        //call the Google web API to take the coordinates and turn them into the name of the location
        private async Task<bool> ReverseGeocode()
        {
            string GoogleKey = "AIzaSyCTQFfvVgPT7Czy8ddpbAzVo1QB2y894ws";
            bool result;

            try
            {
                var httpClient = new HttpClient();
                var requestString = string.Format(@"https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&key={2}",
                                                    lat.ToString(), lng.ToString(), GoogleKey);
                var responseString = await httpClient.GetStringAsync(requestString);

                string[] items = await ParseMapResponse(responseString);

                labelCityState.Text = items[0] + ", " + items[1];

                GPSCounty = CleanCountyString(items[2]);
                labelCounty.Text = GPSCounty + " County";
                result = true;
            }
            catch
            {
                //TODO Handle the error if we can't connect to the net or Google doesn't respond in time
                result = false;
            }
            return result;
        }

        //take what Google gave us and turn it into actual strings
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

        //Removes the word " County" plus anything else we discover to normalize the county string we get from Google
        private string CleanCountyString(string county)
        {
            //TODO: Need to normalize the string to a specific capitalization format, so that we don't fail because sometimes google returns, "Foo county"
            if (county != null)
            {
                List<string> badStrings = new List<string>() { " County" };

                foreach (string s in badStrings)
                {
                    if (county.Contains(s))
                    {
                        county = county.Substring(0, county.LastIndexOf(s));
                    }
                }
            }
            return county;
        }

        //Check that the county we found is a valid CA county, and then set it in the ViewModel 
        private void SetUserCounty()
        {
            App.PlantData.TargetPlant.TargetCounty = Data.Counties.All;
            if (GPSCounty != null)
            {
                try
                {
                    int index = CountyList.FindIndex(x => x.Equals(GPSCounty));

                    if (index > 0)
                    {
                        App.PlantData.TargetPlant.TargetCounty = (Data.Counties)index;
                    }
                    else if (index == 0)
                    {
                        //somehow "None" was set, this should never happen
                        throw new System.InvalidOperationException("County string can not be None");
                    }
                    else //index is negative, meaning we have a county but it could not be found
                    {
                        //This should mean the user has a valid location but not in California, or it means
                        //there is a spelling error
                        //TODO: is there some kind of check I can do to determine if there was a spelling error?
                    }
                }
                catch
                {
                    //if error occurred then we shouldn't have to do anything, as all of the variables are 
                    //initialized elsewhere, and error states are expected.
                }
                
            }
        }

        /* Report results to the user. Cases: 
         *   1) If we have a county set in the ViewModel, then all is well.
         *   2) If we have no county in the ViewModel, but we did get a county string, then
         *      the process worked, but the user is not in a valid location. The user needs to pick their location.
         *   3) If we didn't end up with any county string, then something went wrong with the 
         *      geolocation/reverse geocoding. The user has to pick the county themselves.
         */
        private void ReportResults()
        {
            labelStatus.Text = "Your location:";

            if (App.PlantData.TargetPlant.TargetCounty != Data.Counties.All)
            {
                labelResult.Text = "If this is not the location you want, use the Customize controls to choose a different county.";
            }
            else if (GPSCounty != null)
            {
                labelResult.Text = "You are outside the area supported by this app. Use the Customize controls to choose a county within range.";
            }
            else
            {
                labelResult.Text = "Unable to get your location. Please use the Customize controls to pick your county.";
            }
            //This shows the label and also stops the progress indicator
            labelResult.IsVisible = true;

        }

        //Search county is used for searching and autofiltering
        public ObservableCollection<string> searchCounty = new ObservableCollection<string>(CountyList);
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchCounty.Clear();

            foreach (string c in CountyList)
            {
                string clean = c.ToLower();
                if (clean.Contains(e.NewTextValue.ToLower()))
                    searchCounty.Add(c);
            }

            //SearchCounty = ObservableCollection<string>(CountyList.FindAll(x => x.Contains(e.NewTextValue))) ;
        }

        private void SearchBar_ButtonPressed(object sender, EventArgs e)
        {
            //Do I need to do anything here? I assume that when the user types the last char then the above method gets hit
            //if so the value I want is sender.Text
            ;
        }

        private void countyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int index = CountyList.FindIndex(x => x.Equals(e.SelectedItem));

            if (index >= 0)
            {
                App.PlantData.TargetPlant.TargetCounty = (Data.Counties)index;
            }

        }
    }

    
}
