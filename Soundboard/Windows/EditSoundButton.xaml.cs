using NAudio.Wave;
using Soundboard.Controls;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Soundboard.Windows
{
    /// <summary>
    /// Interaktionslogik für ChangeFileWindow.xaml
    /// </summary>
    public partial class EditSoundButton : Window, INotifyPropertyChanged
    {
        public SoundButton SoundButton { get => _soundButton; set { _soundButton = value; } }
        private SoundButton _soundButton;
        private double min;
        private double max;
        private double start;
        private double end;

        public double Min { get => min; set { min = value; OnPropertyChanged(); } }
        public double Max { get => max; set { max = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public EditSoundButton(SoundButton sndButton)
        {
            InitializeComponent();
            DataContext = this;
            Owner = System.Windows.Application.Current.MainWindow;
            SoundButton = sndButton;

            InitSoundFile();
        }

        private void InitSoundFile()
        {
            if (SoundButton.SoundFile == null || !File.Exists(SoundButton.SoundFile)) return;

            AudioFileReader audioFileReader = new AudioFileReader(SoundButton.SoundFile);
            Min = 0d;
            Max = audioFileReader.TotalTime.TotalSeconds;
            if (SoundButton.End == 0.0d || SoundButton.End > Max)
            {
                SoundButton.End = Max = audioFileReader.TotalTime.TotalSeconds;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "c:/";
                ofd.Filter = "mp3 Files (*.mp3)|*.mp3";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SoundButton.SoundFile = ofd.FileName;
                    SoundButton.Start = 0.0d;
                    SoundButton.End = 0.0d;
                    InitSoundFile();
                }
            }
        }

        private void PlayPreview(object sender, RoutedEventArgs e)
        {
            SoundPlayer.PlaySound(SoundButton.SoundFile, SoundButton.Volume, SoundButton.Start, SoundButton.End);
        }
    }
}
