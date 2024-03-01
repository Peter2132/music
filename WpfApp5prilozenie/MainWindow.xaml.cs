using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5prilozenie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> playlistmusic; 
        private int PositionMusic;  

        public MainWindow()
        {
            InitializeComponent();
            InitializeMPlayer();
        }

        private void InitializeMPlayer()
        {
            playlistmusic = new List<string>();
            PositionMusic = 0;
            media.MediaEnded += Media_MediaEnded;
        }

        private void OpenPapka_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string selectFolder = dialog.FileName;
                LoadAudioFiles(selectFolder);
                PlayTrack();
            }
        }

        private void LoadAudioFiles(string folderPath)
        {
            playlistmusic = Directory.GetFiles(folderPath, "*.mp3").ToList();
            Example.ItemsSource = playlistmusic.Select(System.IO.Path.GetFileName);
        }

        private void PlayTrack()
        {
            if (playlistmusic.Count > 0 && PositionMusic < playlistmusic.Count)
            {
                media.Source = new Uri(playlistmusic[PositionMusic]);
                media.Play();
            }
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (PositionMusic < playlistmusic.Count - 1)
            {
                Next_Click(null, null);
            }
            else
            {
                PositionMusic = 0;
                PlayTrack();
            }
        }
        private void Slider_Valuechenged(object sender, RoutedEventArgs e)
        {
            media.Position = TimeSpan.FromSeconds(Slider.Value);
        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            Slider.Maximum = media.NaturalDuration.HasTimeSpan ? media.NaturalDuration.TimeSpan.TotalSeconds : 0;
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, _) =>
            {
                if (media.NaturalDuration.HasTimeSpan)
                {
                    TimeInfo.Text = $"{media.Position:mm\\:ss} / {media.NaturalDuration.TimeSpan:mm\\:ss}";
                    Slider.Value = media.Position.TotalSeconds;
                }
            };
            timer.Start();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
            
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
             media.Pause();
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            RestartTrack();
        }

        private void RestartTrack()
        {
            media.Position = TimeSpan.Zero;  
            PlayTrack();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            PositionMusic = (PositionMusic + 1) % playlistmusic.Count;
            PlayTrack();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            PositionMusic = (PositionMusic - 1 + playlistmusic.Count) % playlistmusic.Count;
            media.Position = TimeSpan.Zero; 
            PlayTrack();
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            playlistmusic = playlistmusic.OrderBy(x => random.Next()).ToList();
            PlayTrack();
        }

        private void SliderVoice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = SliderVoice.Value;
        }

        private void HistoryLine_Click(object sender, RoutedEventArgs e)
        {
            ShowHistory();
        }

        private void ShowHistory()
        {
            HistoryWindow historyWindow = new HistoryWindow(playlistmusic);
            historyWindow.PlayTrack += HistoryWindow_PlayTrack;
            historyWindow.ShowDialog();
        }

        private void HistoryWindow_PlayTrack(int trackMusicPos)
        {
            PositionMusic = trackMusicPos;
            PlayTrack();
        }


        private void Example_SelectMusic(object sender, SelectionChangedEventArgs e)
        {
            if (Example.SelectedItem != null)
            {
                int selectPositionMusic = Example.SelectedIndex;
                PositionMusic = selectPositionMusic;
                PlayTrack();
            }
        }


    }
}
