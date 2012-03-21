using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ApiCore;
using ApiCore.Friends;

namespace FriendMonitor
{
    public partial class MainWnd : Form
    {
        private string appTitle = "Friend Monitor";

        private SessionInfo sessionInfo;
        private ApiManager manager;
        private FriendsFactory factory;

        private bool isLoggedIn = false;
        private bool downloadInProgress = false;
        private bool exitPending = false;
        private bool userIdIsIncorrect = true;

        private int friendItemsCount = 200;
        private int friendCurrentOffset = 0;
        private int friendStep = 50; // сколько друзей загружать за 1 этап

        private Friend selectedFriend;

        private List<Friend> friendList;


        public MainWnd()
        {
            InitializeComponent();
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
                this.Text = this.appTitle + ": Authorization success!";

                this.factory = new FriendsFactory(this.manager);

            }

        }

        private void MainWnd_Load(object sender, EventArgs e)
        {
            this.Reauth();
        }

        private void reloadFriendsFromSite()
        {
            this.friendList = factory.Get(Convert.ToInt32(this.UserId.Text), FriendNameCase.Nominative, null, null, null, new string[] { FriendFields.FirstName, FriendFields.LastName });
            this.FriendsList.DataSource = this.friendList;
            //foreach (Friend f in this.friendList)
            //{
            //    this.FriendsList.Items.Add(f.ToString());
            //}
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.reloadFriendsFromSite();
        }
    }
}
