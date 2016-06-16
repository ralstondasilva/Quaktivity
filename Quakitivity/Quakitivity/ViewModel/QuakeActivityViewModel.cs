using Quakitivity.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.ViewModel
{
    class QuakeActivityViewModel
    {
        City margao = new City("Margao", "India");
        City panjim = new City("Panjim", "India");
        City vasco = new City("Vasco", "India");

        public ObservableCollection<Earthquake> EarthquakeActivity {get;} = new ObservableCollection<Earthquake>();

        public QuakeActivityViewModel()
        {
            EarthquakeActivity.Add(new Earthquake { Time = DateTime.Now, Magnitude = 7.2, Coordinates = new Point(12.34, 23.5) });
            EarthquakeActivity.Add(new Earthquake { Time = DateTime.Now, Magnitude = 5.2, Coordinates = new Point(132.34, 4323.5) });
            EarthquakeActivity.Add(new Earthquake { Time = DateTime.Now, Magnitude = 3.6, Coordinates = new Point(1232.34, 1123.5) });
        }
    }
}
