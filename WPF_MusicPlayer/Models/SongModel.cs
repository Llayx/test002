using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MusicPlayer.Models
{
    internal class SongModel
    {
        public int Index { get; set; }
        public string Cover { get; set; }
        public string SongName { get; set; }
        public string FilePath { get; set; }
        public string Singer { get; set; }
        public string Duration { get; set; }
        public string Url { get; set; }

    }
}
