using System;
using System.Drawing;
using System.Windows.Forms;

namespace Breeze_gui_frost
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			fiddlerCtrl1.startFiddler();
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
			Invoke(new Action(() =>
			{
				ListViewItem item = listView1.Items.Add(new ListViewItem(new string[] {
				Convert.ToString(listView1.Items.Count),
				oSession.oRequest.headers.UriScheme,
				oSession.oRequest.headers.RequestPath,
				MIMEtype,
				oSession.oResponse.headers.HTTPResponseCode.ToString()
				})
				{
					ForeColor = Color.Green,
					Tag = oSession
				});
				item.EnsureVisible();
			}));
			return;
		}

		private void browserInterface1_LoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		   var attribute= rslib_Ctrl.myOrbit.UsingParserCk(e.Url.AbsoluteUri);
		   Logger.Push(attribute.ToString(), 0);
			if (attribute == rslib_Ctrl.myOrbit.vServiceKind.No) {
			   listView2.Clear();
			   return;
		   }
		   else
		   {
			   var v =getvideoandshow(e.Url.AbsoluteUri);
		   }
		}

		#region
		private bool getvideoandshow(string arg)
		{
			var dic = rslib_Ctrl.myOrbit.UsingParserMain(arg);
			listView2.Clear();
			foreach (var item in dic)
			{
				listView2.Items.Add(new ListViewItem(new string[] { item.Key.ToString(), item.Value.Item1, item.Value.Item2.AbsoluteUri }) {Tag = item.Value });
			} 
				return true;
		}
		#endregion

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			var i = ((ListView)sender).SelectedItems;
			if (i.Count == 0) { return; }
			queueResolver1.factory((Fiddler.Session)i[0].Tag, new proxy());
		}

		private void listView2_DoubleClick(object sender, EventArgs e)
		{
			var s = ((ListView)sender).SelectedItems[0];
			//Todo: 選択不可にする           if (s.) {s.Enabled = false;} else {
			queueResolver1.factory((Tuple<string,Uri,string>)s.Tag, new dlQueue());
			//}
		}
	}
}
