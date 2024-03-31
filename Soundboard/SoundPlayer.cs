using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Soundboard
{
    public static class SoundPlayer
    {
        private static WaveOut _player = new WaveOut();

        public static void PlaySound(string filePath, float volume, double startMS = 0, double endMS = long.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            _player.Pause();

            AudioFileReader audioFileReader = new AudioFileReader(filePath);
            OffsetSampleProvider trimmed = new OffsetSampleProvider(audioFileReader);
            trimmed.SkipOver = TimeSpan.FromMilliseconds(startMS*1000);
            trimmed.Take = TimeSpan.FromMilliseconds((endMS-startMS)*1000);

            _player.Volume = volume;
            _player.Init(trimmed);
            _player.Play();
        }

        public static void StopSound()
        {
            _player.Stop();
        }

        public static void SetPlaybackDevice(int index)
        {
            _player.DeviceNumber = index;
        }

        public static void DisposePlayer()
        {
            _player.Dispose();
        }
    }
}
