using Newtonsoft.Json;
using Soundboard.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Soundboard.Controls
{
    /// <summary>
    /// Interaktionslogik für SoundButton.xaml
    /// </summary>

    [JsonObject(MemberSerialization.OptIn)]
    public partial class SoundButton : UserControl, INotifyPropertyChanged
    {
        [JsonProperty]
        public string SoundFile { get => _soundFile; set { _soundFile = value; OnPropertyChanged(); ButtonName = SoundFile?.Substring(SoundFile.LastIndexOf("\\") + 1); } }

        [JsonProperty]
        public string ButtonName { get => _buttonName; set { _buttonName = value; OnPropertyChanged(); } }

        public bool DeleteMode { get => _deleteMode; set { _deleteMode = value; OnPropertyChanged(); } }

        [JsonProperty]
        public float Volume { get => _volume; set { _volume = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public SoundButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PlaySound(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SoundFile))
            {
                SelectFile();
                return;
            }

            if (!File.Exists(SoundFile))
            {
                ButtonName = "File not found";
                return;
            }


            SoundPlayer.PlaySound(SoundFile, Volume, 0, int.MaxValue);
        }

        private void ChangeFile(object sender, RoutedEventArgs e)
        {
            SelectFile();
        }

        private void SelectFile()
        {
            var changeFileWnd = new EditSoundButton(this);
            changeFileWnd.ShowDialog();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _soundFile;
        private string _buttonName;
        private bool _deleteMode;
        private float _volume;
    }
}
