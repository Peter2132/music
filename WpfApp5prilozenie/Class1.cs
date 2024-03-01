using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp5prilozenie
{
    public partial class HistoryWindow : Window
    {
        public event Action<int> PlayTrack;

        private List<string> playlist;

        public HistoryWindow(List<string> playlist)
        {
            InitializeComponent();
            this.playlist = playlist;
            InitializeHistoryList();
        }

        private void InitializeHistoryList()
        {
            List<string> mp3Filename = new List<string>();

            foreach (string path in playlist)
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);


                if (System.IO.Path.GetExtension(path).Equals(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    mp3Filename.Add(filename);
                }
            }
            HistoryListBox.ItemsSource = mp3Filename;
        }

        private void HistoryListBox_SelectFromHistory(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryListBox.SelectedItem != null)
            {
                int selectPositionMusic = HistoryListBox.SelectedIndex;
                if (selectPositionMusic >= 0)
                {
                    PlayTrack?.Invoke(selectPositionMusic);
                    DialogResult = true;
                }
            }
        }




    }
}
