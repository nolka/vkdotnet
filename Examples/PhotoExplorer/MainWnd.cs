using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using ApiCore;
using ApiCore.Friends;
using ApiCore.Photos;
using HttpDownloader;

namespace PhotoExplorer
{
    public partial class MainWnd : Form
    {
        private string appTitle = "Photo Explorer";
        //private string version = "0.1";

        private string downloadLabelTemplate = "{0}/{1} File: {2}";
        private string pathToPhotosStore = null;

        private SessionInfo sessionInfo;
        private ApiManager manager;

        private bool isLoggedIn = false;
        private bool downloadInProgress = false;
        private bool exitPending = false;
        private bool userIdIsIncorrect = true;

        private int friendItemsCount = 200;
        private int friendCurrentOffset = 0;
        private int itemsToDownload = 0;
        private int photosInAlbum = 0;
        private int itemsDownloaded = 1;

        private Friend selectedFriend;
        private AlbumEntry selectedAlbum;

        private List<Friend> friendList;
        private List<AlbumEntry> albumList;
        private List<PhotoEntryFull> photoList;

        private HttpDownloaderFactory httpDownloader;
        private FriendsFactory friendFactory;
        private PhotosFactory photoFactory;

        private Regex userIdCheck;


        public MainWnd()
        {
            InitializeComponent();
        }

        private void MainWnd_Resize(object sender, EventArgs e)
        {
            this.DownloadPanel.Top = this.Height / 2 - this.DownloadPanel.Height / 2;
            this.DownloadPanel.Left = this.Width / 2 - this.DownloadPanel.Width / 2;
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopDownload();
        }

        private void MainWnd_Shown(object sender, EventArgs e)
        {
            this.Text = this.appTitle + ": Not Authorized!";
            this.Reauth();
            this.GetFriendList();
        }

        private void reloadAudioListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AlbumsList.Items.Clear();
            this.albumList.Clear();
            this.GetFriendList();
        }

        private void FriendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedFriend = this.friendList[((ListBox)sender).SelectedIndex];
            this.LoadUserAlbums(this.selectedFriend.Id);
        }

        private void AlbumsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedAlbum = this.albumList[((ListBox)sender).SelectedIndex];
            this.LoadAlbumPhotos(this.selectedAlbum);
        }

        private void UserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UserId.Text != "" && this.userIdIsIncorrect == false)
                {
                    this.LoadUserAlbums(Convert.ToInt32(this.userIdCheck.Match(UserId.Text).Value));
                }
                else
                {
                    MessageBox.Show("Entered user id is incorrect!");
                }
            }
        }

        private void UserId_TextChanged(object sender, EventArgs e)
        {
            if(this.userIdCheck.IsMatch(((ToolStripTextBox)sender).Text))
            {
                 ((ToolStripTextBox)sender).BackColor = SystemColors.Window;
                 this.userIdIsIncorrect = false;
            }
            else
            {
                ((ToolStripTextBox)sender).BackColor = Color.LightCoral;
                this.userIdIsIncorrect = true;
            }
        }

        private void reauthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Reauth();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowAboutBox();
        }

        private long DownloadFile(string url, string path)
        {
            try
            {
                this.ItemName.Text = string.Format(this.downloadLabelTemplate,this.itemsDownloaded, this.photosInAlbum, Path.GetFileName(url));
                httpDownloader = new HttpDownloaderFactory(url);
                httpDownloader.DownloadProgress += (s, e) =>
                {
                    this.CurrentProgress.Value = (int)e.PercentComplete;
                    Application.DoEvents();
                };
                return httpDownloader.DownloadToFile(path + Path.GetFileName(url), true);
            }
            catch 
            {
                return 0;
            }
        }

        private void DownloadAlbum(AlbumEntry album, List<PhotoEntryFull> photos)
        {
            try
            {
                if (this.pathToPhotosStore == null)
                {
                    if (FB.ShowDialog() == DialogResult.OK)
                    {
                        this.pathToPhotosStore = FB.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                }
                    Regex r = new Regex(@"[^A-Za-zA-Яа-я0-9\ \,\.\!\+\=\)\(\&\%\$\#\@\-\{\}\[\]]");
                    Regex ra = new Regex(@"[^A-Za-zA-Яа-я0-9\ \!\+\=\)\(\&\%\$\#\@\-\{\}\[\]]");
                    string path = this.pathToPhotosStore + "\\" + ((this.selectedFriend != null) ? r.Replace(this.selectedFriend.ToString(), "_") : UserId.Text) + "\\" + ra.Replace(album.Title, "_") + "\\";
                    
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    this.photosInAlbum = album.Size;
                    this.downloadInProgress = true;
                    stopDownloadingToolStripMenuItem.Enabled = true;
                    this.TotalProgress.Maximum = album.Size;
                    this.DownloadPanel.Visible = true;
                    this.TotalProgress.Value = 0;
                    this.FriendList.Enabled = false;
                    this.AlbumsList.Enabled = false;

                    this.itemsDownloaded = 1;

                    for (int friendId = 0; friendId < photos.Count; friendId++)
                    {
                        if (!this.exitPending)
                        {
                            if (Small.Checked && photos[friendId].UrlSmall != "")
                            {
                                this.DownloadFile(photos[friendId].UrlSmall, path);
                            }
                            if (Big.Checked && photos[friendId].UrlBig != "")
                            {
                                this.DownloadFile(photos[friendId].UrlBig, path);
                            }
                            if (Thumb.Checked && photos[friendId].Url != "")
                            {
                                this.DownloadFile(photos[friendId].Url, path);
                            }
                            if (X.Checked && photos[friendId].UrlXBig != "")
                            {
                                this.DownloadFile(photos[friendId].UrlXBig, path);
                            }
                            if (Xx.Checked && photos[friendId].UrlXXBig != "")
                            {
                                this.DownloadFile(photos[friendId].UrlXXBig, path);
                            }

                                this.TotalProgress.Value++;
                                this.itemsDownloaded++;
                        }
                    }

                    this.DownloadPanel.Visible = false;
                    this.FriendList.Enabled = true;
                    this.AlbumsList.Enabled = true;
                    this.downloadInProgress = false;

            }
            catch (Exception e)
            {
                this.StopDownload();
                MessageBox.Show("Some errors ocurred while downloading photos:\n" + e.Message);
            }
        }

        private void Reauth()
        {
            if (!this.isLoggedIn)
            {
                SessionManager sm = new SessionManager(1928531, Convert.ToInt32(ApiPerms.Audio | ApiPerms.ExtendedMessages | ApiPerms.ExtendedWall | ApiPerms.Friends | ApiPerms.Offers | ApiPerms.Photos | ApiPerms.Questions | ApiPerms.SendNotify | ApiPerms.SidebarLink | ApiPerms.UserNotes | ApiPerms.UserStatus | ApiPerms.Video | ApiPerms.WallPublisher | ApiPerms.Wiki));
                //sm.Log += new SessionManagerLogHandler(sm_Log);
                this.sessionInfo = sm.GetSession();
                if (this.sessionInfo != null)
                {
                    this.isLoggedIn = true;
                }
            }

            if (this.isLoggedIn)
            {
                manager = new ApiManager(this.sessionInfo);
                //manager.Log += new ApiManagerLogHandler(manager_Log);
                //manager.DebugMode = true;
                manager.Timeout = 10000;
                this.FriendList.Enabled = true;
                this.Text = this.appTitle + ": Authorization success!";

                this.friendFactory = new FriendsFactory(this.manager);
                this.photoFactory = new PhotosFactory(this.manager);
                this.photoList = new List<PhotoEntryFull>();
                this.albumList = new List<AlbumEntry>();
                this.userIdCheck = new Regex("([\\d])+$");
                this.GetFriendList();
            }

        }

        private void GetFriendList()
        {
            try
            {
                this.AlbumsList.Enabled = false;
                this.FriendList.Items.Clear();
                this.friendList = this.friendFactory.Get( Convert.ToInt32(UserId.Text), FriendNameCase.Nominative, null, null, null, new string[] { "uid", "first_name", "nickname", "last_name" });
                for (int i = 0; i < this.friendList.Count; i++)
                {
                    this.FriendList.Items.Add(this.friendList[i]);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error loading friend list:\n"+e.Message);
            }
        }

        private void LoadUserAlbums(int friendId)
        {
            try
            {
                this.AlbumsList.Items.Clear();
                this.albumList = this.photoFactory.GetAlbums(friendId, null);
                for (int i = 0; i < this.albumList.Count; i++)
                {
                    this.AlbumsList.Items.Add(this.albumList[i]);
                }
                this.AlbumsList.Enabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading friend albums:\n" + e.Message);
            }
        }

        private void LoadAlbumPhotos(AlbumEntry album)
        {
            List<PhotoEntryFull> photoList = this.photoFactory.GetPhotos(album.OwnerId, album.Id, null, null, null);
            if (photoList.Count > 0)
            {
                this.DownloadAlbum(album, photoList);
            }
        }

        private void ShowAboutBox()
        {
            Process p = new Process();
            p.StartInfo.FileName = "http://xternalx.com";
            p.Start();
        }

        private void StopDownload()
        {
            this.exitPending = true;
            if (this.httpDownloader != null)
            {
                this.httpDownloader.Stop();
            }
            this.DownloadPanel.Visible = false;
            stopDownloadingToolStripMenuItem.Enabled = false;
            this.downloadInProgress = false;
            this.AlbumsList.Enabled = true;
            this.FriendList.Enabled = true;
        }

        private void stopDownloadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StopDownload();
        }

        private void downloadAllFriendsPhotosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FriendsProgress.Maximum = this.friendList.Count;
            this.FriendsProgress.Value = 0;
            foreach (Friend f in this.friendList)
            {
                this.selectedFriend = f;
                this.LoadUserAlbums(f.Id);
                foreach(AlbumEntry a in this.albumList)
                {
                    this.LoadAlbumPhotos(a);
                }
                this.FriendsProgress.Value++;
            }
        }

        



    }
}
