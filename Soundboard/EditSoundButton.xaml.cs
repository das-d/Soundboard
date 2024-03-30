using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Soundboard
{
    /// <summary>
    /// Interaktionslogik für ChangeFileWindow.xaml
    /// </summary>
    public partial class EditSoundButton : Window, INotifyPropertyChanged
    {
        public SoundButton SoundButton { get => _soundButton; set { _soundButton = value; } }
        private SoundButton _soundButton;

        public event PropertyChangedEventHandler? PropertyChanged;

        public EditSoundButton(SoundButton sndButton)
        {
            InitializeComponent();
            DataContext = this;
            Owner = System.Windows.Application.Current.MainWindow;
            SoundButton = sndButton;
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
                }
            }
        }
    }
}
