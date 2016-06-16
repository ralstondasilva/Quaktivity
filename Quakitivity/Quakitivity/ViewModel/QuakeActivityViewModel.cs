﻿using Quakitivity.Model;
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

        public ObservableCollection<Model.Earthquake> EarthquakeActivity {get;} = new ObservableCollection<Model.Earthquake>();

        public QuakeActivityViewModel()
        {
            //EarthquakeActivity.Add(new Model.Earthquake { Time = DateTime.Now, Magnitude = 7.2, Coordinates = new Point(12.34, 23.5) });
            //EarthquakeActivity.Add(new Model.Earthquake { Time = DateTime.Now, Magnitude = 5.2, Coordinates = new Point(132.34, 4323.5) });
            //EarthquakeActivity.Add(new Model.Earthquake { Time = DateTime.Now, Magnitude = 3.6, Coordinates = new Point(1232.34, 1123.5) });

            StartPeriodicTimer();
        }

        public void StartPeriodicTimer()
        {
            //Start Periodic Health Check
            DispatcherTimer periodicTimer = new DispatcherTimer(DispatcherPriority.Normal);
            periodicTimer.Interval = Settings.Default.RefreshPeriod;
            periodicTimer.Tick += new EventHandler(async (object s, EventArgs a) =>
            {
                GeoJsonSummary summary = await EarthquakeUsgsGovRestAPI.GetAllHourSummary();
                foreach (var earthquake in summary.Earthquakes)
                {
                    EarthquakeActivity.Add(new Model.Earthquake { Time = earthquake.Time, Magnitude = earthquake.Magnitude, Coordinates = earthquake.Coordinates });
                }
            });
            periodicTimer.Start();
        }
    }
}