using HtmlAgilityPack;
using Prism.Commands;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_MusicPlayer.Models;

namespace WPF_MusicPlayer.ViewModels
{
    internal class FirstPageViewModel
    {
        public ObservableCollection<AlbumModel> AlbumList { get; set; } = new ObservableCollection<AlbumModel>();
        public ObservableCollection<SongModel> NewList { get; set; } = new ObservableCollection<SongModel>();
        public ObservableCollection<SongModel> HotList { get; set; } = new ObservableCollection<SongModel>();
        public ObservableCollection<SongModel> Top500List { get; set; } = new ObservableCollection<SongModel>();

        
        WebClient wc = new WebClient();
        public FirstPageViewModel()
        {

            // 数据来源于网站
            // 网站的数据获取   两种：专门的数据接口     /   没有接口   网页解析      /
            HtmlDocument htmlDoc = new HtmlDocument();
            wc.Encoding = Encoding.UTF8;


            #region 初始化专辑数据
            string htmlStr = wc.DownloadString("https://music.migu.cn/v3/music/new_album");
            htmlDoc.LoadHtml(htmlStr);

            HtmlNodeCollection liNodes = htmlDoc.DocumentNode
                .SelectNodes("//div[@class='thumb']");

            if (liNodes != null && liNodes.Count > 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var node = liNodes[i].ChildNodes[1];
                    var a = node.ChildNodes[1];
                    var img = a.ChildNodes[1];
                    var src = img.Attributes["data-src"].Value;


                    node = liNodes[i].ChildNodes[3];
                    var name = node.ChildNodes[1].ChildNodes[1].InnerText;
                    var author = node.ChildNodes[3].ChildNodes[1].InnerText;
                    var id = node.ChildNodes[5].Attributes["data-id"].Value;


                    AlbumList.Add(new AlbumModel
                    {
                        Id = id,
                        Cover = "https:" + src,
                        Author = author,
                        Title = name,
                    });
                }
            }
            #endregion

            #region 初始化 NewList
            htmlStr = wc.DownloadString("https://www.kugou.com/yy/rank/home/1-6666.html?from=rank");
            htmlDoc.LoadHtml(htmlStr);
            var listNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='pc_temp_songlist  pc_rank_songlist_short']");
            var ulNode = listNode.ChildNodes[1];
            for (int i = 1; i <= 30; i += 2)
            {
                var name = ulNode.ChildNodes[i].ChildNodes[9].InnerText;    // 歌名 - 演唱者
                string[] items = name.Trim().Replace("\t", "").Replace("\n", "").Split('-');
                var url = ulNode.ChildNodes[i].ChildNodes[9].Attributes["href"].Value; // 歌曲详情页地址（里面可以取到mp3地址）

                NewList.Add(new SongModel
                {
                    Index = (i + 1) / 2,
                    SongName = items[0].Trim(),
                    Singer = items[1].Trim(),
                    Url = url
                });
            }
            #endregion

            #region 初始化执歌榜
            htmlStr = wc.DownloadString("https://www.kugou.com/yy/rank/home/1-52144.html?from=rank");
            htmlDoc.LoadHtml(htmlStr);
            listNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='pc_temp_songlist ']");
            ulNode = listNode.ChildNodes[1];
            // li
            for (int i = 1; i <= 30; i += 2)
            {
                var name = ulNode.ChildNodes[i].ChildNodes[7].InnerText;
                var url = ulNode.ChildNodes[i].ChildNodes[7].Attributes["href"].Value;
                string[] items = name.Trim().Replace("\t", "").Replace("\n", "").Split('-');
                string duration = ulNode.ChildNodes[i].ChildNodes[9].ChildNodes[7].InnerText.Trim();
                HotList.Add(new SongModel
                {
                    Index = (i + 1) / 2,
                    SongName = items[0],
                    Singer = items[1],
                    Duration = duration,
                    Url = url
                });
            }
            #endregion

            #region 初始化执歌榜
            htmlStr = wc.DownloadString("https://www.kugou.com/yy/rank/home/1-8888.html?from=rank");
            htmlDoc.LoadHtml(htmlStr);
            listNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='pc_temp_songlist ']");
            ulNode = listNode.ChildNodes[1];
            // li
            for (int i = 1; i <= 30; i += 2)
            {
                var name = ulNode.ChildNodes[i].ChildNodes[7].InnerText;
                var url = ulNode.ChildNodes[i].ChildNodes[7].Attributes["href"].Value;
                string[] items = name.Trim().Replace("\t", "").Replace("\n", "").Split('-');
                string duration = ulNode.ChildNodes[i].ChildNodes[9].ChildNodes[7].InnerText.Trim();
                Top500List.Add(new SongModel
                {
                    Index = (i + 1) / 2,
                    SongName = items[0],
                    Singer = items[1],
                    Duration = duration,
                    Url = url
                });
            }
            #endregion

        }
    }
}
