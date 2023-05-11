using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MusicPlayer.Models
{
    internal class AlbumModel
    {
        /// <summary>
        /// 专辑
        /// </summary>
        public string Id { get; set; }
        public string Cover { get; set; }//封面
        public string Title { get; set; }//专辑名
        public string Author { get; set; }//歌手
        public string TargetUrl { get; set; }//点击封面跳转页面链接
    }
}
