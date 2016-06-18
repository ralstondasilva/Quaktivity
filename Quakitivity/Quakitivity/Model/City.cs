using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.Model
{
    /// <summary>
    /// The details about a city
    /// (Assuming that these details will not change during the lifetime of the app,
    /// since we cache the csv file which has the details about the cities)
    /// </summary>
    class City 
    {
        public string Name { get; }
        public string Country { get; }
        public Point Coordinates { get; }
        public City(string name, string country, Point coordinates) { Name = name; Country = country; Coordinates = coordinates;}
    }
}
