using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Media;
using WPF_MusicPlayer.Models;

namespace WPF_MusicPlayer.ViewModels
{
    internal class MainViewModel:BindableBase
    {
        //private ObservableCollection<SongSheetModel> sList;
        //public ObservableCollection<SongSheetModel> SList
        //{
        //    get { return sList; }
        //    set { sList = value; }
        //}

        private double volume = 0.4;
        public double Volume
        {
            get { return volume; }
            set { volume = value; RaisePropertyChanged(); }
        }

        private double progressMax;
        public double ProgressMax
        {
            get { return progressMax; }
            set { progressMax = value; }
        }

        MediaPlayer player = new MediaPlayer();

        MediaTimeline timeline = new MediaTimeline();

        public ObservableCollection<SongSheetModel> SList { get; set; } = new ObservableCollection<SongSheetModel>();

        //播放列表
        public ObservableCollection<SongModel> PlayList { get; set; } = new ObservableCollection<SongModel>();

        public DelegateCommand<SongModel> PlayCommand { get; set; }

        public DelegateCommand<SongModel> PlayDownloadCommand { get; set; }

        public MainViewModel()
        {
            //SList = new ObservableCollection<SongSheetModel>() 
            //{
            //  new SongSheetModel() { Header = "默认歌单", Icon = "\ue688" },
            //  new SongSheetModel() { Header = "本地音乐", Icon = "\ue635" },
            //};
            SList.Add(new SongSheetModel() { Header = "默认歌单", Icon = "\ue688" });
            SList.Add(new SongSheetModel() { Header = "本地音乐", Icon = "\ue635" });
            SList.Add(new SongSheetModel() { Header = "自定义歌单", Icon = "\ue63e" });

            try
            {
                string playJson = System.IO.File.ReadAllText("./playlist.json");
                PlayList = new ObservableCollection<SongModel>(System.Text.Json.JsonSerializer.Deserialize<List<SongModel>>(playJson));
            }
            catch
            {

            }

            PlayCommand = new DelegateCommand<SongModel>(Play);

            PlayDownloadCommand = new DelegateCommand<SongModel>(PlayDownload);

            player.MediaOpened += PlayerMediaOpened;
        }
        private void PlayerMediaOpened(object sender, EventArgs e)
        {
            this.ProgressMax = player.NaturalDuration.TimeSpan.TotalSeconds;
        }
        private async void PlayDownload(SongModel song)
        {
            //下载MP3文件
            string filePath = "";
            var options = new LaunchOptions { Headless = true };
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            using (var wc = new WebClient())
            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync(song.Url);
                var jsSelectAllAnchors = @"Array.from(document.querySelectorAll('audio')).map(a => a.src);";
                var urls = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);

                if (urls.Length > 0)
                {
                    filePath = "./songs/" + song.SongName + ".mp3";
                    wc.DownloadFile(urls[0], filePath);
                }
            }

            PlayList.Add(new SongModel()
            {
                SongName  = song.SongName,
                Singer = song.Singer,
                Duration = song.Duration,
                FilePath = filePath,
            });

            //保存播放列表
            string listJson = System.Text.Json.JsonSerializer.Serialize(PlayList);
            System.IO.File.WriteAllText("./playlist.json", listJson);
        }
        private void Play(SongModel song)
        {
            if (player.Source == null || string.IsNullOrEmpty(player.Source.ToString()))
            {
                Uri uri = new Uri(song.FilePath);
                
            }
        }
    }
}
