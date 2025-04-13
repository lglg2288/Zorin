using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNovelEngine
{
    internal class Page
    {
        public string Character;
        public string Text;
        public string BGImagePath;
        public int? BGWidth;
        public int? BGHeight;
        public int? BGPositionX;
        public int? BGPositionY;
    }
    public class MusicEntry
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string ActionType { get; set; }   // Play, Pause, Stop
        public string MusicType { get; set; }    // Background, Effect
        public int Volume { get; set; }          // 0-100
        public int PageID { get; set; }
    }

}