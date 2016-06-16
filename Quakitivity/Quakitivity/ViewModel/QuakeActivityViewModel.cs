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

namespace Quakitivity.ViewModel
{
    class QuakeActivityViewModel
    {
        //City margao = new City("Margao", "India");
        //City panjim = new City("Panjim", "India");
        //City vasco = new City("Vasco", "India");
        public RelayCommand RefreshQuakeActivity { get; set; }

        /// <summary>
        /// We use a combination of an ObservableCollection and a Dictionary to store the earthquakes.
        /// We save a pointer to the same object in both. When there are new items, we quickly check the dictionary if the item is present.
        /// If the new item has an updated timestamp, we replace the item in the ObservableCollection.
        /// </summary>
        public ObservableCollection<Model.Earthquake> EarthquakeActivity {get;} = new ObservableCollection<Model.Earthquake>();
        private IDictionary<string, Model.Earthquake> EarthquakeActivityDictionary = new Dictionary<string, Model.Earthquake>();

        public QuakeActivityViewModel()
        {
            //Model.Earthquake quake = new Model.Earthquake { Time = DateTime.Now, Magnitude = 7.2, Coordinates = new Point(12.34, 23.5), UpdatedTime = DateTime.MinValue };
            //EarthquakeActivityDictionary.Add("ci37389175", quake);
            //EarthquakeActivity.Add(quake);

            RefreshQuakeActivity = new RelayCommand(FetchEarthquakeActivity);
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
                        EarthquakeActivity.Remove(item);
                        EarthquakeActivityDictionary[id.First()] = quake;
                        EarthquakeActivity.Add(quake);

                    }
                }
                else
                {
                    //This is the first time we are seeing this quake, so add it
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
