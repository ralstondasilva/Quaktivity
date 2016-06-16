using Quakitivity.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Quakitivity.NetworkCommunication
{
    class EarthquakeUsgsGovRestAPI
    {
        public static async Task<GeoJsonSummary> GetAllHourSummary()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string requestURL = Settings.Default.SummaryURL;
                    HttpResponseMessage response = await client.GetAsync(requestURL);
                    response.EnsureSuccessStatusCode();

                    GeoJsonSummary geoJsonSummary = null;
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        //String response
                        //string responseString = await response.Content.ReadAsStringAsync();
                        //responseStream.Seek(0, SeekOrigin.Begin);

                        //Convert JSON to tokenResponse
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GeoJsonSummary));
                        geoJsonSummary = (GeoJsonSummary)ser.ReadObject(responseStream);
                    }
                    return geoJsonSummary;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
