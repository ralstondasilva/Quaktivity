using Quakitivity.Helpers;
using Quakitivity.Model;
using Quakitivity.NetworkCommunication;
using Quakitivity.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static Quakitivity.Helpers.CityListGenerator;


namespace Quakitivity.ViewModel
{
    class QuakeActivityViewModel
    {
        private City[] Cities;

        public RelayCommand RefreshQuakeActivity { get; set; }
        public RelayCommand WindowLoaded { get; set; }

        /// <summary>
        /// We use a combination of an ObservableCollection and a Dictionary to store the earthquakes.
        /// We save a pointer to the same object in both. When there are new items, we quickly check the dictionary if the item is present.
        /// If the new item has an updated timestamp, we replace the item in the ObservableCollection.
        /// </summary>
        public ObservableCollection<Model.Earthquake> EarthquakeActivity {get;} = new ObservableCollection<Model.Earthquake>();
        private IDictionary<string, Model.Earthquake> EarthquakeActivityDictionary = new Dictionary<string, Model.Earthquake>();

        public QuakeActivityViewModel()
        {
            RefreshQuakeActivity = new RelayCommand(FetchEarthquakeActivity);
            WindowLoaded = new RelayCommand(Initialize);  
        }

        private async Task FetchCityInfo()
        {
            string filePath = await FileHelper.FetchFile();
            string extractedPath = await FileHelper.ExtractFile(filePath);   
            Cities = await GetCities(extractedPath);
        }

        private async void Initialize(object parameter)
        {
            await FetchCityInfo();
            FetchEarthquakeActivity(null);
            StartPeriodicTimer();
        }

        private async void FetchEarthquakeActivity(object parameter)
        {
            GeoJsonSummary summary = await EarthquakeUsgsGovRestAPI.GetAllHourSummary();
            if (summary == null) return;

            foreach (var earthquake in summary?.Earthquakes)
            {
                Model.Earthquake quake = new Model.Earthquake { Time = earthquake.Time, Magnitude = earthquake.Magnitude, Coordinates = earthquake.Coordinates, UpdatedTime = earthquake.UpdatedTime };

                var id = EarthquakeActivityDictionary.Keys.Intersect(earthquake.AssociatedIds);
                if (id.Any())
                {
                    //If there is an associated ID, just update the quake info if it has a newer update timestamp
                    var item = EarthquakeActivityDictionary[id.First()];
                    if (earthquake.UpdatedTime > item.UpdatedTime)
                    {
                        quake.Cities = new ObservableCollection<City>(await CityHelper.FindNearbyCities(quake.Coordinates, Cities, Settings.Default.NearbyCityCount));
                        EarthquakeActivity.Remove(item);
                        EarthquakeActivityDictionary[id.First()] = quake;
                        EarthquakeActivity.Add(quake);
                    }
                }
                else
                {
                    //This is the first time we are seeing this quake, so add it
                    quake.Cities = new ObservableCollection<City>(await CityHelper.FindNearbyCities(quake.Coordinates, Cities, Settings.Default.NearbyCityCount));
                    EarthquakeActivityDictionary.Add(earthquake.ID, quake);
                    EarthquakeActivity.Add(quake);
                }
            }
        }

        public void StartPeriodicTimer()
        {
            DispatcherTimer periodicTimer = new DispatcherTimer(DispatcherPriority.Normal);
            periodicTimer.Interval = Settings.Default.RefreshPeriod;
            periodicTimer.Tick += new EventHandler((object s, EventArgs a) => { FetchEarthquakeActivity(null); });
            periodicTimer.Start();
        }
    }
}
