using System.ComponentModel;
using System.Windows;

namespace Quakitivity.Model
{
    class UiState : INotifyPropertyChanged
    {
        public int RefreshPeriod { get; } = (int) Properties.Settings.Default.RefreshPeriod.TotalMinutes;

        /// <summary>
        /// Visibility of the progress message, while the list of cities is being downloaded
        /// </summary>
        private Visibility cityDownloader = Visibility.Visible;
        public Visibility CityDownloader
        {
            get { return cityDownloader; }
            set { if (cityDownloader != value) { cityDownloader = value; RaisePropertyChanged(nameof(CityDownloader)); } }
        }

        /// <summary>
        /// Visibility of the earthquake result table
        /// </summary>
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
