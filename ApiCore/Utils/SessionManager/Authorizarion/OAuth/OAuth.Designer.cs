namespace ApiCore
{
    partial class OAuth
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
            this.LoginBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // LoginBrowser
            // 
            this.LoginBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoginBrowser.Location = new System.Drawing.Point(0, 0);
            this.LoginBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.LoginBrowser.Name = "LoginBrowser";
            this.LoginBrowser.ScrollBarsEnabled = false;
            this.LoginBrowser.Size = new System.Drawing.Size(573, 549);
            this.LoginBrowser.TabIndex = 0;
            this.LoginBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.LoginBrowser_Navigated);
            this.LoginBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.LoginBrowser_Navigating);
            // 
            // BrowserAuthorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 549);
            this.Controls.Add(this.LoginBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrowserAuthorization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.Shown += new System.EventHandler(this.LoginWnd_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser LoginBrowser;
    }
}