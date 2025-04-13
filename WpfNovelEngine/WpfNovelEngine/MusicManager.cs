using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfNovelEngine
{
    static class MusicManager
    {
        private class ActiveMusicPlayer
        {
            public MediaPlayer Player { get; set; }
            public string Path { get; set; }
        }

        static private List<ActiveMusicPlayer> activePlayers = new List<ActiveMusicPlayer>();
        static private DataBase db = new DataBase();

        public static void init(string Storyline, int NumPage, bool isDesign)
        {
            int? PageId = db.GetPageId(Storyline, NumPage);

            if (PageId == null)
            {
                if (isDesign)
                    MessageBox.Show($"Don't found pageId with prop Storyline: {Storyline}, Pagenum: {NumPage}");
                return;
            }

            var currentMusics = db.GetMusicsForPage((int)PageId);

            if (currentMusics == null || currentMusics.Count == 0)
            {
                if (isDesign)
                    MessageBox.Show("Музыка не найдена.");
                return;
            }

            foreach (var music in currentMusics)
            {
                if (!File.Exists(music.Path))
                {
                    if (isDesign)
                        MessageBox.Show($"Файл не найден: {music.Path}");
                    continue;
                }

                var player = new MediaPlayer();

                switch (music.ActionType)
                {
                    case "▶ Play":
                    case "Play":
                        player.Open(new Uri(music.Path, UriKind.Absolute));
                        player.Volume = music.Volume / 100.0;
                        player.Play();
                        AddPlayer(player, music.Path);
                        break;
                    case "⏸ Pause":
                    case "Pause":
                        foreach (var item in activePlayers.Where(p => p.Path == music.Path).ToList())
                            item.Player.Pause();
                        break;
                    case "⏹ Stop":
                    case "Stop":
                        foreach (var item in activePlayers.Where(p => p.Path == music.Path).ToList())
                        {
                            item.Player.Stop();
                            item.Player.Close();
                            activePlayers.Remove(item);
                        }
                        break;
                }
            }
        }
        public static void AddPlayer(MediaPlayer player, string path)
        {
            player.MediaEnded += RemovePlayer;
            activePlayers.Add(new ActiveMusicPlayer() { Player = player, Path = path });
        }

        private static void RemovePlayer(object sender, EventArgs e)
        {
            if (sender is MediaPlayer player)
            {
                var match = activePlayers.FirstOrDefault(p => p.Player == player);
                if (match != null)
                {
                    match.Player.Stop();
                    match.Player.Close();
                    activePlayers.Remove(match);
                }
            }
        }


        public static void StopAll()
        {
            foreach (var player in activePlayers)
            {
                player.Player.Stop();
                player.Player.Close();
            }
            activePlayers.Clear();
        }
    }
}
