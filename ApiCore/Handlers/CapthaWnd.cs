using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ApiCore.Handlers
{
    public partial class CapthaWnd : Form
    {
        private string capthaUrl;
        private string capthaHash;

        private ApiManager manager;

        public CapthaWnd(ApiManager manager, string capthaUrl, string capthaHash)
        {
            this.manager = manager;
            this.capthaUrl = capthaUrl;
            this.capthaHash = capthaHash;

            InitializeComponent();

            this.CapthaImage.ImageLocation = this.capthaUrl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.resend();
        }

        private void resend()
        {
            this.Hide();
            this.manager.Params("method", this.manager.LastMethod);
            this.manager.Params("captha_key", this.CapthaText.Text);
            this.manager.Params("captha_sid", this.capthaHash);
            this.manager.Execute();
            this.Close();
        }

        private void CapthaText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                this.resend();
        }

        private void CapthaWnd_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.manager.Cancel();
        }
        
    }
}
