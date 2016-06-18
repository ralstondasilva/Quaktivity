using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quakitivity.Model
{
    class UiState : INotifyPropertyChanged
    {
        private Visibility cityDownloader = Visibility.Visible;
        public Visibility CityDownloader
        {
            get { return cityDownloader; }
            set { if (cityDownloader != value) { cityDownloader = value; RaisePropertyChanged(nameof(CityDownloader)); } }
        }

        private Visibility earthquakeReport = Visibility.Collapsed;
        public Visibility EarthquakeReport
        {
            get { return earthquakeReport; }
            set { if (earthquakeReport != value) { earthquakeReport = value; RaisePropertyChanged(nameof(EarthquakeReport)); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
