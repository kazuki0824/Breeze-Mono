using System;
using System.Windows.Forms;

namespace Breeze_gui_frost
{
    public partial class View_prototype : UserControl
    {
#region
        static void AddToList(View_prototype src)
        {

        }
        static async void Serialize()
        {
            //Todo:
        }
#endregion


        public View_prototype()
        {
            InitializeComponent();
        }
        internal void RegisterCancellationtoken(IDisposable d)
        {
            
        }
        internal void MarkAsCompleted()
        {
            this.label2.Text = "SUCCESS";

            //Todo
        }
    }
}
