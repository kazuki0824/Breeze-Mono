using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breeze_gui_frost
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void fiddlerロードToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fiddlerCtrl1.startFiddler();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            fiddlerCtrl1.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fiddlerCtrl1_FiddlerCaughtNormalMedia( Fiddler.Session oSession)
        {
            // Index, Scheme, Request, MIME, Cookie
            var MIMEtype = oSession.oResponse.MIMEType;
            ListViewItem item = listView1.Items.Add(new ListViewItem(new string[] {
	            Convert.ToString(listView1.Items.Count),
	            oSession.oRequest.headers.UriScheme,
	            oSession.oRequest.headers.RequestPath,
	            MIMEtype,
                oSession.oResponse.headers.HTTPResponseCode.ToString()
                }) {
	                    ForeColor = Color.Green,
	                       Tag = oSession
                    });
                item.EnsureVisible();
            return;
        }

        private void browserInterface1_LoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           var attribute= rslib_Ctrl.myOrbit.UsingParserCk(e.Url.AbsoluteUri);
           if (attribute == rslib_Ctrl.myOrbit.vServiceKind.No) {
               listView2.Clear();
               return;
           }
           else
           {
               getvideoandshow(e.Url.AbsoluteUri);
           }
        }

        #region
        private bool getvideoandshow(string arg)
        {
            var dic = rslib_Ctrl.myOrbit.UsingParserMain(arg);
            listView2.Clear();
            foreach (var item in dic)
            {
                listView2.Items.Add(new ListViewItem(new string[] {item.Key.ToString(),item.Value.Item1,item.Value.Item2.AbsoluteUri}));
            } 
                return true;
        }
        #endregion
    }
}
