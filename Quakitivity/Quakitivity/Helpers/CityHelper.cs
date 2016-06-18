using Quakitivity.Model;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.Helpers
{
    class CityHelper
    {
        public static async Task<City[]> FindNearbyCities(Point point, City[] cities, int maxCityCount)
        {
            return await Task.Run(() => 
            {
                SortedDictionary<double, City> nearestCities = new SortedDictionary<double, City>();

                //Insert the first 3 cities
                for (int i = 0; i < maxCityCount && i < cities.Count(); i++) nearestCities.Add(DistanceBetween(point, cities[i].Coordinates),cities[i]);

                //For the remaining, add only if it is nearer than the cities already in nearestCities;
                for (int i = maxCityCount; i < cities.Count(); i++)
                {
                    double distance = DistanceBetween(point, cities[i].Coordinates);
                    double maxDistance = nearestCities.Last().Key;

                    if (distance < maxDistance)
                    {
                        nearestCities.Remove(maxDistance);
                        nearestCities.Add(distance, cities[i]);
                    }
                }

                return nearestCities.Values.ToArray();
            });
        }

        public static double DistanceBetween(Point location1, Point location2)
        {
            GeoCoordinate coordinate1 = new GeoCoordinate(location1.X, location1.Y);
            GeoCoordinate coordinate2 = new GeoCoordinate(location2.X, location2.Y);
            return coordinate1.GetDistanceTo(coordinate2);
        }
    }
}
