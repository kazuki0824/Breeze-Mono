using System;

namespace Breeze_gui_frost
{
    public partial class proxy : View_prototype
    {
        public proxy()
        {
            InitializeComponent();
        }
        string[] c = new[] { "／", "―", "＼", "｜" };
        int num;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (InvokeRequired) { this.Invoke(new Action(updateText)); } else { this.updateText(); }
        }
        private void updateText()
        {
             label2.Text = c[num++ % 4];
        }
    }
}
