using Quakitivity.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.Helpers
{
    class CityListGenerator
    {
        /// <summary>
        /// Reads a csv containg the cities of the world and returns an array of City objects 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<City[]> GetCities(string filePath)
        {
            try
            {
                IDictionary<Point, City> cities = new Dictionary<Point, City>();
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    //Ignore the first line (header)
                    await streamReader.ReadLineAsync();

                    while (!streamReader.EndOfStream)
                    {
                        City city = ReadCity(await streamReader.ReadLineAsync());

                        //If this city doesnt exist in the dictionary, add it.
                        //NOTE: We only save the first instance of the city that we have seen
                        if (city!=null && !cities.ContainsKey(city.Coordinates)) cities.Add(city.Coordinates, city);
                    }
                }
    
                return cities.Values.ToArray();
            }
            catch
            {
                //TODO: Log error
                return null;
            }
        }

        /// <summary>
        /// Convert a line of text to a City object. The Cities are described in the following format
        /// ISO 3166-1 country code, FIPS 5-2 subdivision code, GNS FD, GNS UFI, ISO 639-1 language code, language script, name, latitude, longitude
        /// GB,J8,PPLA,-2604469,en,latin,"Nottingham",52.966667,-1.166667
        /// </summary>
        /// <param name="cityString"></param>
        /// <returns></returns>
        private static City ReadCity(string cityString)
        {
            try
            {
                string[] cityDetails = cityString.Split(',');
                string name = cityDetails[6].Replace("\"","");
                string country = cityDetails[0];
                Point coordinates = new Point(Double.Parse(cityDetails[7]), Double.Parse(cityDetails[8]));
                return new City(name, country, coordinates);
            }
            catch
            {
                //TODO: Log error
                return null;
            }
        }
    }
}
