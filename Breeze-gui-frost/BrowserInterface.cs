using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Breeze_gui_frost
{
    public partial class BrowserInterface : UserControl
    {
        public BrowserInterface()
        {
            InitializeComponent();
            this.LoadCompleted += setUiState;
        }

        public delegate void LoadCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e);
        public event LoadCompletedEventHandler LoadCompleted;

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (((WebBrowser)sender).Url == e.Url)
            {
                if (LoadCompleted != null)
                {
                    LoadCompleted(sender, e);
                }
            }
        }
        private void setUiState(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBox1.Text = e.Url.AbsoluteUri;
            var c = (WebBrowser)sender;
            button1.Enabled = c.CanGoBack;
            button2.Enabled = c.CanGoForward;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
                      if (e.KeyCode == Keys.Enter && Uri.IsWellFormedUriString(((TextBox)sender).Text, UriKind.Absolute)) { e.Handled = false; webBrowser1.Navigate(((TextBox)sender).Text); }
        }
    }
}
