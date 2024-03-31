using NAudio.Wave;
using Newtonsoft.Json;
using Soundboard.Controls;
using Soundboard.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soundboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string CONFIG_FILE = ".\\config.json";
        public bool DeleteMode { get => _deleteMode; set { _deleteMode = value; OnPropertyChanged(); } }
        public ObservableCollection<SoundButton> SoundButtons { get => _soundButtons; set { _soundButtons = value; OnPropertyChanged(); } }
        public ObservableCollection<PlaybackDevice> PlaybackDevices { get => _playbackDevices; set { _playbackDevices = value; OnPropertyChanged(); } }
        public PlaybackDevice SelOutputDevice { get => _selOutputDevice; set { _selOutputDevice = value; OnPropertyChanged(); SoundPlayer.SetPlaybackDevice(SelOutputDevice?.DeviceIndex ?? -1);  } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Init();
        }

        private void Init()
        {
            SoundButtons = new ObservableCollection<SoundButton>();

            if (File.Exists(CONFIG_FILE))
            {
                var config = File.ReadAllText(CONFIG_FILE);
                SoundButtons = JsonConvert.DeserializeObject<ObservableCollection<SoundButton>>(config);
            }

            this.Height = Properties.Settings.Default.Height;
            this.Width = Properties.Settings.Default.Width;

            GetPlaybackDevices();
            SetPlaybackDevice();
        }

        private void GetPlaybackDevices()
        {
            PlaybackDevices = new ObservableCollection<PlaybackDevice>();
            PlaybackDevices.Add(new PlaybackDevice() { DeviceName = "Default", DeviceIndex = -1 });
            for (int i = 0; i < NAudio.Wave.WaveOut.DeviceCount; ++i)
            {
                string devName = NAudio.Wave.WaveOut.GetCapabilities(i).ProductName;
                PlaybackDevices.Add(new PlaybackDevice() { DeviceName = devName, DeviceIndex = i });
            }
        }

        private void SetPlaybackDevice()
        {
            var deviceName = Properties.Settings.Default.DeviceName;
            SelOutputDevice = PlaybackDevices.Where(x => x.DeviceName.Contains(deviceName)).FirstOrDefault();
            if (SelOutputDevice == null)
            {
                SelOutputDevice = PlaybackDevices.FirstOrDefault();
            }
        }

        private void StopSound(object sender, RoutedEventArgs e)
        {
            SoundPlayer.StopSound();
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            SoundButtons.Add(new SoundButton() { ButtonName = "New Button", Volume = 1.0f, Start = 0.0d, End = 0.0d});
        }

        private void DeleteButtonMode(object sender, RoutedEventArgs e)
        {
            if (!DeleteMode)
            {
                DeleteMode = true;
            }
            else
            {
                DeleteMode = false;
            }

            foreach (var button in SoundButtons)
            {
                button.DeleteMode = DeleteMode;

                if (DeleteMode)
                {
                    button.MouseDown += DeleteButton;
                }
                else
                {
                    button.MouseDown -= DeleteButton;
                }   
            }
        }

        private void DeleteButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is SoundButton sndButton)
            {
                sndButton.MouseDown -= DeleteButton;
                SoundButtons.Remove(sndButton);
            }
        }

        private void WndClosed(object sender, EventArgs e)
        {
            //Save Config
            var output = JsonConvert.SerializeObject(SoundButtons);
            File.WriteAllText(CONFIG_FILE, output);

            Properties.Settings.Default.Height = this.Height;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.DeviceName = SelOutputDevice?.DeviceName ?? "NONE";
            Properties.Settings.Default.Save();

            SoundPlayer.DisposePlayer();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<SoundButton> _soundButtons;
        private bool _deleteMode;
        private ObservableCollection<PlaybackDevice> _playbackDevices;
        private PlaybackDevice _selOutputDevice;
    }
}