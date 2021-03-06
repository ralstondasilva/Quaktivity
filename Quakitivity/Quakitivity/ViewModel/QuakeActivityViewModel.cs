﻿using Quakitivity.Helpers;
using Quakitivity.Model;
using Quakitivity.NetworkCommunication;
using Quakitivity.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Quakitivity.Helpers;


namespace Quakitivity.ViewModel
{
    class QuakeActivityViewModel
    {
        private City[] Cities;
        private bool FirstLoad = true;

        public RelayCommand RefreshQuakeActivity { get; set; }
        public RelayCommand WindowLoaded { get; set; }

        public UiState UiState { get; } = new UiState();

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

        /// <summary>
        /// Gets all the cities of the world
        /// </summary>
        /// <returns></returns>
        private async Task FetchCityInfo()
        {
            string filePath = await FileHelper.FetchFile(Properties.Settings.Default.CitiesURL);
            string extractedPath = FileHelper.ExtractFile(filePath);   
            Cities = await CityListGenerator.GetCities(extractedPath);
        }

        /// <summary>
        /// This function is triggered when the window is loaded
        /// </summary>
        /// <param name="parameter"></param>
        private async void Initialize(object parameter)
        {
            if (this.FirstLoad)
            {
                this.FirstLoad = false;

                UiState.CityDownloader = Visibility.Visible;
                UiState.EarthquakeReport = Visibility.Collapsed;
                await FetchCityInfo();
                UiState.CityDownloader = Visibility.Collapsed;
                UiState.EarthquakeReport = Visibility.Visible;

                FetchEarthquakeActivity(null);
                
                StartPeriodicTimer();
            }
        }

        /// <summary>
        /// Fetches earthquake activity and finds the list of nearby cities
        /// </summary>
        /// <param name="parameter"></param>
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

        /// <summary>
        /// Periodic timer to check for earthquake activity
        /// </summary>
        public void StartPeriodicTimer()
        {
            DispatcherTimer periodicTimer = new DispatcherTimer(DispatcherPriority.Normal);
            periodicTimer.Interval = Settings.Default.RefreshPeriod;
            periodicTimer.Tick += new EventHandler((object s, EventArgs a) => { FetchEarthquakeActivity(null); });
            periodicTimer.Start();
        }
    }
}
