using System;
using System.Windows.Forms;

namespace Breeze_gui_frost
{
    public partial class BrowserInterface : UserControl
    {
        public BrowserInterface()
        {
            InitializeComponent();
            this.LoadCompleted += setUiState;
            this.button1.Click += (sender, e) => webBrowser1.GoBack();
            this.button1.Click += (sender, e) => webBrowser1.GoForward();
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
                      if (e.KeyCode == Keys.Enter && Uri.IsWellFormedUriString(((TextBox)sender).Text, UriKind.Absolute)) { e.Handled = false; webBrowser1.Navigate(((TextBox)sender).Text); }
        }
    }
}
