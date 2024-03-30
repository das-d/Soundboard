using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundboard
{
    public static class SoundPlayer
    {
        private static WaveOut _player = new WaveOut();

        public static void PlaySound(string filePath, float volume, long startMS = 0, long endMS = long.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            if (_player.PlaybackState == PlaybackState.Playing)
            {
                _player.Stop();
                return;
            }


            AudioFileReader audioFileReader = new AudioFileReader(filePath);
            OffsetSampleProvider trimmed = new OffsetSampleProvider(audioFileReader);
            trimmed.SkipOver = TimeSpan.FromMilliseconds(startMS);
            trimmed.Take = TimeSpan.FromMilliseconds(endMS);

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
