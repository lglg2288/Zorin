using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Media;

namespace WpfNovelEngine
{
    class Mechanics
    {
        private MediaPlayer mediaPlayer;
        private string musicPath;

        public void InitMusic(string musicPath)
        {
            if (!File.Exists(musicPath))
            {
                MessageBox.Show("File not found!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(musicPath));
        }

        public void InitMusicAction(string action)
        {
            if (mediaPlayer.Source == null)
            {
                MessageBox.Show("The audio file is not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            switch (action)
            {
                case "▶ Play":
                    mediaPlayer.Play();
                    break;
                case "⏸ Pause":
                    mediaPlayer.Pause();
                    break;
                case "⏹ Stop":
                    mediaPlayer.Stop();
                    break;
                default:
                    MessageBox.Show("Select an action!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
            return;
        }
    }
}
