using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.Model
{
    class Earthquake : INotifyPropertyChanged
    {
        /// <summary>
        /// The time when the earthquake occured.
        /// </summary>
        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { if (time != value) { time = value; RaisePropertyChanged(nameof(Time)); } }
        }

        /// <summary>
        /// The time when the earthquake info was updated.
        /// </summary>
        private DateTime updatedTime;
        public DateTime UpdatedTime
        {
            get { return updatedTime; }
            set { if (updatedTime != value) { updatedTime = value; RaisePropertyChanged(nameof(UpdatedTime)); } }
        }

        /// <summary>
        /// The magnitude of the earthquake
        /// </summary>
        private double magnitude;
        public double Magnitude
        {
            get { return magnitude; }
            set { if (magnitude != value) { magnitude = value; RaisePropertyChanged(nameof(Magnitude)); } }
        }

        /// <summary>
        /// The coordinates of the place where the earthquake occured 
        /// (We only store the surface coordinates and not the depth at which the earthquake occured)
        /// </summary>
        private Point coordinates;
        public Point Coordinates
        {
            get { return coordinates; }
            set { if (coordinates != value) { coordinates = value; RaisePropertyChanged(nameof(Coordinates)); } }
        }

        /// <summary>
        /// The list of cities closest to the epicentre of the earthquake
        /// </summary>
        public ObservableCollection<City> Cities { get; set; } = new ObservableCollection<City>(); 

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
