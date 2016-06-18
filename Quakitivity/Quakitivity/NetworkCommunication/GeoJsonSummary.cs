using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace Quakitivity.NetworkCommunication
{
    [DataContract]
    class GeoJsonSummary
    {
        [DataMember(Name = "type")]
        private string Type { get; set; }

        [DataMember(Name = "metadata")]
        private Metadata Metadata { get; set; }

        [DataMember(Name = "features")]
        private Earthquake[] earthquakes { get; set;}

        public Earthquake[] Earthquakes
        {
            get
            {
                if (Type != "FeatureCollection" || Metadata?.Status != 200) return null;
                return earthquakes.Where(e => e?.Type == "Feature" && e?.Properties?.Type == "earthquake").ToArray();
            }
        }
    }

    [DataContract]
    class Metadata
    {
        [DataMember(Name = "status")]
        public int Status { get; private set; }
    }

    [DataContract]
    class Earthquake
    {
        [DataMember(Name = "id")]
        public string ID { get; private set; }

        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "properties")]
        public Properties Properties { get; private set; }

        [DataMember(Name = "geometry")]
        private Geometry Geometry { get; set; }

        public double Magnitude => Properties.Magnitude;

        public DateTime Time => new DateTime(1970, 1, 1).AddMilliseconds(Properties.Time); //Ignoring leap seconds, since we dont have the info

        public DateTime UpdatedTime => new DateTime(1970, 1, 1).AddMilliseconds(Properties.Updated); //Ignoring leap seconds, since we dont have the info

        public Point Coordinates => (Geometry?.Coordinates?.Length == 3) ? new Point(Geometry.Coordinates[1], Geometry.Coordinates[0]) : new Point(0, 0);

        public string[] AssociatedIds => Properties.AssociatedIds.Split(',').Where((id) => !string.IsNullOrEmpty(id)).ToArray();
    }

    [DataContract]
    class Properties
    {
        [DataMember(Name = "mag")]
        public double Magnitude { get; private set; }

        [DataMember(Name = "time")]
        public long Time { get; private set; }

        [DataMember(Name = "updated")]
        public long Updated { get; private set; }

        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "ids")]
        public string AssociatedIds { get; private set; }
    }

    [DataContract]
    class Geometry
    {
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "coordinates")]
        public double[] Coordinates { get; private set; }
    }
}
