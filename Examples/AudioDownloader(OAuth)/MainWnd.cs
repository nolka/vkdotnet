using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using ApiCore;
using ApiCore.Audio;
using ApiCore.Friends;
using ApiCore.Messages;
using ApiCore.Photos;
using ApiCore.Questions;
using ApiCore.Status;
using ApiCore.Wall;
using HttpDownloader;

namespace AudioDownloader
{
    public partial class MainWnd : Form
    {
        private string appTitle = "Audio Downloader";
        //private string version = "0.7";

        private SessionInfo sessionInfo;
        private ApiManager manager;

        private bool isLoggedIn = false;
        private bool downloadInProgress = false;
        private bool exitPending = false;

        private int itemsToDownload = 0;

        private List<AudioEntry> audioList;

        private HttpDownloaderFactory httpDownloader;
        private AudioFactory audioFactory;

        private int searchItemsCount = 200;
        private int searchCurrentOffset = 0;

        public MainWnd()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.downloadInProgress)
            {
                Download();
            }
            else
            {
                MessageBox.Show("Downloading already in progress");
            }
        }

        private void reauthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reauth();
        }

        private void reloadAudioListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadAudio();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutBox();
        }

        // аццке метод загрузки файлов
        private void Download()
        {
            try
            {
                if (FB.ShowDialog() == DialogResult.OK)
                {
                    this.downloadInProgress = true;
                    this.itemsToDownload = this.AudioList.CheckedItems.Count;
                    this.TotalProgress.Maximum = this.itemsToDownload;
                    this.DownloadPanel.Visible = true;
                    this.TotalProgress.Value = 0;
                    this.AudioList.Enabled = false;

                    int itemsDownloaded = 1;

                    for (int itemId = 0; itemId < this.AudioList.Items.Count; itemId++)
                    {
                        if (!this.exitPending)
                        {
                            if (this.AudioList.GetItemCheckState(itemId) == CheckState.Checked)
                            {
                                AudioEntry a = this.AudioList.Items[itemId] as AudioEntry;
                                this.SongName.Text = Convert.ToString(itemsDownloaded) + "/" + this.itemsToDownload.ToString() + " " + a.ToString();

                                httpDownloader = new HttpDownloaderFactory(a.Url);
                                httpDownloader.DownloadProgress += (s, e) =>
                                    {

                                        this.CurrentProgress.Value = (int)e.PercentComplete;
                                        Application.DoEvents();
                                    };
                                // Убираем из названий песен все символы, кроме букв алфавита, знаков препинания,
                                // и прочих разрешенных 
                                Regex r = new Regex(@"[^A-Za-zA-Яа-я0-9\ \,\.\!\+\=\)\(\&\%\$\#\@\-\{\}\[\]]");
                                httpDownloader.DownloadToFile(FB.SelectedPath + "\\" + r.Replace(a.ToString(), "") + Path.GetExtension(a.Url));

                                this.TotalProgress.Value++;
                                this.AudioList.SetItemChecked(itemId, false);
                                itemsDownloaded++;
                            }
                        }
                    }

                    this.DownloadPanel.Visible = false;
                    this.AudioList.Enabled = true;
                    this.downloadInProgress = false;
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                this.DownloadPanel.Visible = false;
                this.AudioList.Enabled = true;
                this.downloadInProgress = false;
                MessageBox.Show("Some errors ocurred while downloading songs:\n"+e.Message);
            }
        }

        private void Reauth()
        {
            // если пользователь не авторизован
            if (!this.isLoggedIn)
            {
                // создаем новый менеджер сессий
                SessionManager sm = new SessionManager(1928531, "audio");
                // подключаем обработчик события на получение сообщений из лога
                //sm.Log += new SessionManagerLogHandler(sm_Log);
                // Авторизуемся через OAuth и получаем сессию
                this.sessionInfo = sm.GetOAuthSession();
                // если сессия получена, отмечаем пользователя как залогинившегося
                if (this.sessionInfo != null)
                {
                    this.isLoggedIn = true;
                }
            }

            // если пользователь залогинился 
            if (this.isLoggedIn)
            {
                // создаем менеджера api. через этот объект происходит взаимодействие всех фрапперов api
                manager = new ApiManager(this.sessionInfo);
                //manager.Log += new ApiManagerLogHandler(manager_Log);
                //manager.DebugMode = true;
                // устанавливаем таймаут для запросов к pi
                manager.Timeout = 10000;
                // косметические изменения
                this.Text = this.appTitle + ": Authorization success!";
                // создаем фабрику аудиозаписей
                this.audioFactory = new AudioFactory(this.manager);

            }
            
        }

        private void ReloadAudio()
        {
            try
            {
                if (this.isLoggedIn)
                {
                    this.AudioList.Items.Clear();
                    this.audioList = audioFactory.Get(null, null, null);
                    foreach (AudioEntry a in this.audioList)
                    {
                        this.AudioList.Items.Add(a);
                        Application.DoEvents();
                    }
                }
                else
                {
                    MessageBox.Show("You are not authorized!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while loading audio list: \n"+e.Message);
            }
        }

        private void ShowAboutBox()
        {
            Process p = new Process();
            p.StartInfo.FileName = "http://xternalx.com";
            p.Start();
        }

        private void Search(int count, int offset)
        {
            this.audioList.Clear();
            this.AudioList.Items.Clear();
            this.audioList = this.audioFactory.Search(SearchBox.Text, AudioSortOrder.ByDate, false, count, offset);
            foreach (AudioEntry a in this.audioList)
            {
                this.AudioList.Items.Add(a);
                Application.DoEvents();
            }
        }

    private void MainWnd_Shown(object sender, EventArgs e)
    {
        this.AudioList.CheckOnClick = true;
        this.Text = this.appTitle + ": Not Authorized!";
        this.Reauth();
        this.ReloadAudio();
    }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.AudioList.Items.Count; i++)
            {
                this.AudioList.SetItemChecked(i, true);
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.AudioList.Items.Count; i++)
            {
                this.AudioList.SetItemChecked(i, false);
            }
        }

        private void MainWnd_Resize(object sender, EventArgs e)
        {
            this.DownloadPanel.Top = this.Height / 2 - this.DownloadPanel.Height / 2;
            this.DownloadPanel.Left = this.Width / 2 - this.DownloadPanel.Width / 2;
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.exitPending = true;
            if (this.httpDownloader != null)
            {
                this.httpDownloader.Stop();
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Text != "")
            {
                PrevPage.Visible = true;
                NextPage.Visible = true;
            }
            else
            {
                PrevPage.Visible = false;
                NextPage.Visible = false;
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.searchCurrentOffset = 0;
                this.Search(this.searchItemsCount, this.searchCurrentOffset);
            }
        }

        private void NextPage_Click(object sender, EventArgs e)
        {
            this.searchCurrentOffset += this.searchItemsCount;
            this.Search(this.searchItemsCount, this.searchCurrentOffset);
        }

        private void PrevPage_Click(object sender, EventArgs e)
        {
            if (this.searchCurrentOffset > 0 && (this.searchCurrentOffset - this.searchItemsCount) >= 0)
            {
                this.searchCurrentOffset -= this.searchItemsCount;
                this.Search(this.searchItemsCount, this.searchCurrentOffset);
            }
            else
            if ((this.searchCurrentOffset - this.searchItemsCount) < 0)
            {
                this.searchCurrentOffset = 0; ;
                this.Search(this.searchItemsCount, this.searchCurrentOffset);
            }
        }


        public void sm_Log(string msg)
        {
        }
    }
}
