﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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
        [DataMember(Name = "type")]
        public string Type { get; private set; }

        [DataMember(Name = "properties")]
        public Properties Properties { get; private set; }

        [DataMember(Name = "geometry")]
        private Geometry Geometry { get; set; }

        public double Magnitude => Properties.Magnitude;

        public DateTime Time => new DateTime(1970, 1, 1).AddMilliseconds(Properties.Time); //TODO: Ignoring leap seconds, since we dont have the info

        public Point Coordinates => (Geometry?.Coordinates?.Length == 3) ? new Point(Geometry.Coordinates[1], Geometry.Coordinates[2]) : new Point(0, 0);

    }

    [DataContract]
    class Properties
    {
        [DataMember(Name = "mag")]
        public double Magnitude { get; private set; }

        [DataMember(Name = "time")]
        public long Time { get; private set; }

        [DataMember(Name = "type")]
        public string Type { get; private set; }
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