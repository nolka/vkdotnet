namespace PhotoExplorer
{
    partial class MainWnd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
            this.FriendList = new System.Windows.Forms.ListBox();
            this.DownloadPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CurrentProgress = new System.Windows.Forms.ProgressBar();
            this.TotalProgress = new System.Windows.Forms.ProgressBar();
            this.ItemName = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopDownloadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.reauthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAudioListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AlbumsList = new System.Windows.Forms.ListBox();
            this.FB = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.UserId = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Small = new System.Windows.Forms.ToolStripButton();
            this.Thumb = new System.Windows.Forms.ToolStripButton();
            this.Big = new System.Windows.Forms.ToolStripButton();
            this.X = new System.Windows.Forms.ToolStripButton();
            this.Xx = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.FriendsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.downloadAllFriendsPhotosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FriendList
            // 
            this.FriendList.Dock = System.Windows.Forms.DockStyle.Left;
            this.FriendList.FormattingEnabled = true;
            this.FriendList.Location = new System.Drawing.Point(0, 0);
            this.FriendList.Name = "FriendList";
            this.FriendList.Size = new System.Drawing.Size(161, 434);
            this.FriendList.TabIndex = 0;
            this.FriendList.SelectedIndexChanged += new System.EventHandler(this.FriendList_SelectedIndexChanged);
            // 
            // DownloadPanel
            // 
            this.DownloadPanel.Controls.Add(this.tableLayoutPanel1);
            this.DownloadPanel.Location = new System.Drawing.Point(119, 172);
            this.DownloadPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DownloadPanel.Name = "DownloadPanel";
            this.DownloadPanel.Padding = new System.Windows.Forms.Padding(10);
            this.DownloadPanel.Size = new System.Drawing.Size(382, 113);
            this.DownloadPanel.TabIndex = 4;
            this.DownloadPanel.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.CurrentProgress, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.TotalProgress, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ItemName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(362, 93);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // CurrentProgress
            // 
            this.CurrentProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentProgress.Location = new System.Drawing.Point(13, 53);
            this.CurrentProgress.Name = "CurrentProgress";
            this.CurrentProgress.Size = new System.Drawing.Size(336, 27);
            this.CurrentProgress.TabIndex = 0;
            // 
            // TotalProgress
            // 
            this.TotalProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TotalProgress.Location = new System.Drawing.Point(13, 33);
            this.TotalProgress.Name = "TotalProgress";
            this.TotalProgress.Size = new System.Drawing.Size(336, 14);
            this.TotalProgress.TabIndex = 1;
            // 
            // ItemName
            // 
            this.ItemName.AutoSize = true;
            this.ItemName.Location = new System.Drawing.Point(13, 10);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(16, 13);
            this.ItemName.TabIndex = 2;
            this.ItemName.Text = "...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(592, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stopDownloadingToolStripMenuItem,
            this.downloadAllFriendsPhotosToolStripMenuItem,
            this.toolStripMenuItem2,
            this.reauthToolStripMenuItem,
            this.reloadAudioListToolStripMenuItem,
            this.toolStripMenuItem3,
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // stopDownloadingToolStripMenuItem
            // 
            this.stopDownloadingToolStripMenuItem.Enabled = false;
            this.stopDownloadingToolStripMenuItem.Name = "stopDownloadingToolStripMenuItem";
            this.stopDownloadingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.stopDownloadingToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.stopDownloadingToolStripMenuItem.Text = "Stop downloading!";
            this.stopDownloadingToolStripMenuItem.Click += new System.EventHandler(this.stopDownloadingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // reauthToolStripMenuItem
            // 
            this.reauthToolStripMenuItem.Name = "reauthToolStripMenuItem";
            this.reauthToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.reauthToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.reauthToolStripMenuItem.Text = "Reauth";
            this.reauthToolStripMenuItem.Click += new System.EventHandler(this.reauthToolStripMenuItem_Click);
            // 
            // reloadAudioListToolStripMenuItem
            // 
            this.reloadAudioListToolStripMenuItem.Name = "reloadAudioListToolStripMenuItem";
            this.reloadAudioListToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.reloadAudioListToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.reloadAudioListToolStripMenuItem.Text = "Reload friend list";
            this.reloadAudioListToolStripMenuItem.Click += new System.EventHandler(this.reloadAudioListToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(219, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(219, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DownloadPanel);
            this.panel1.Controls.Add(this.AlbumsList);
            this.panel1.Controls.Add(this.FriendList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 434);
            this.panel1.TabIndex = 6;
            // 
            // AlbumsList
            // 
            this.AlbumsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AlbumsList.Enabled = false;
            this.AlbumsList.FormattingEnabled = true;
            this.AlbumsList.Location = new System.Drawing.Point(161, 0);
            this.AlbumsList.Name = "AlbumsList";
            this.AlbumsList.Size = new System.Drawing.Size(431, 434);
            this.AlbumsList.TabIndex = 5;
            this.AlbumsList.SelectedIndexChanged += new System.EventHandler(this.AlbumsList_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UserId,
            this.toolStripSeparator1,
            this.Small,
            this.Thumb,
            this.Big,
            this.X,
            this.Xx});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(592, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // UserId
            // 
            this.UserId.Name = "UserId";
            this.UserId.Size = new System.Drawing.Size(300, 25);
            this.UserId.ToolTipText = "Custom user ID albums";
            this.UserId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserId_KeyDown);
            this.UserId.TextChanged += new System.EventHandler(this.UserId_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Small
            // 
            this.Small.CheckOnClick = true;
            this.Small.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Small.Image = ((System.Drawing.Image)(resources.GetObject("Small.Image")));
            this.Small.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Small.Name = "Small";
            this.Small.Size = new System.Drawing.Size(23, 22);
            this.Small.Text = "s";
            this.Small.ToolTipText = "Small";
            // 
            // Thumb
            // 
            this.Thumb.CheckOnClick = true;
            this.Thumb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Thumb.Image = ((System.Drawing.Image)(resources.GetObject("Thumb.Image")));
            this.Thumb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Thumb.Name = "Thumb";
            this.Thumb.Size = new System.Drawing.Size(23, 22);
            this.Thumb.Text = "t";
            this.Thumb.ToolTipText = "Thumbnail";
            // 
            // Big
            // 
            this.Big.Checked = true;
            this.Big.CheckOnClick = true;
            this.Big.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Big.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Big.Image = ((System.Drawing.Image)(resources.GetObject("Big.Image")));
            this.Big.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Big.Name = "Big";
            this.Big.Size = new System.Drawing.Size(23, 22);
            this.Big.Text = "b";
            this.Big.ToolTipText = "Starnard";
            // 
            // X
            // 
            this.X.Checked = true;
            this.X.CheckOnClick = true;
            this.X.CheckState = System.Windows.Forms.CheckState.Checked;
            this.X.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.X.Image = ((System.Drawing.Image)(resources.GetObject("X.Image")));
            this.X.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(23, 22);
            this.X.Text = "x";
            this.X.ToolTipText = "Large";
            // 
            // Xx
            // 
            this.Xx.Checked = true;
            this.Xx.CheckOnClick = true;
            this.Xx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Xx.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Xx.Image = ((System.Drawing.Image)(resources.GetObject("Xx.Image")));
            this.Xx.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Xx.Name = "Xx";
            this.Xx.Size = new System.Drawing.Size(23, 22);
            this.Xx.Text = "xx";
            this.Xx.ToolTipText = "VeryLarge";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FriendsProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 483);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(592, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // FriendsProgress
            // 
            this.FriendsProgress.Name = "FriendsProgress";
            this.FriendsProgress.Size = new System.Drawing.Size(150, 16);
            // 
            // downloadAllFriendsPhotosToolStripMenuItem
            // 
            this.downloadAllFriendsPhotosToolStripMenuItem.Name = "downloadAllFriendsPhotosToolStripMenuItem";
            this.downloadAllFriendsPhotosToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.downloadAllFriendsPhotosToolStripMenuItem.Text = "Download all friends photos";
            this.downloadAllFriendsPhotosToolStripMenuItem.Click += new System.EventHandler(this.downloadAllFriendsPhotosToolStripMenuItem_Click);
            // 
            // MainWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 505);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainWnd";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWnd_FormClosing);
            this.Shown += new System.EventHandler(this.MainWnd_Shown);
            this.Resize += new System.EventHandler(this.MainWnd_Resize);
            this.DownloadPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox FriendList;
        private System.Windows.Forms.Panel DownloadPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar CurrentProgress;
        private System.Windows.Forms.ProgressBar TotalProgress;
        private System.Windows.Forms.Label ItemName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reauthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadAudioListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox AlbumsList;
        private System.Windows.Forms.FolderBrowserDialog FB;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox UserId;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton Thumb;
        private System.Windows.Forms.ToolStripButton Small;
        private System.Windows.Forms.ToolStripButton Big;
        private System.Windows.Forms.ToolStripButton X;
        private System.Windows.Forms.ToolStripButton Xx;
        private System.Windows.Forms.ToolStripMenuItem stopDownloadingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem downloadAllFriendsPhotosToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar FriendsProgress;
    }
}

