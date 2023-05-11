using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MusicPlayer.Models
{
    internal class SongSheetModel
    {
        /// <summary>
        /// 歌单列表
        /// </summary>
        
        public string Header { get; set; }//歌单名称
        
        public string Icon { get; set; }//歌单图标

        public List<SongModel> Songs { get; set; } = new List<SongModel>();//歌单下的歌曲集合
    }
}
