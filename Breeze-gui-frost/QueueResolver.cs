using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AsynchronousExtensions;
using Fiddler;


namespace Breeze_gui_frost
{
    public partial class QueueResolver : Component
    {
        internal queueController queue_Instance = new queueController();
        internal CompositeDisposable cancellationMan = new CompositeDisposable(); //Todo:
        public QueueResolver()
        {
            InitializeComponent();
            reg_event();
        }

        void queue_Instance_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int index = 0;
            var controlArray = ((queueController)sender).tmp2;
            if (this.NormalQueuePanel != null) { this.NormalQueuePanel.Invoke(new Action(() => this.NormalQueuePanel.Controls.AddRange(controlArray))); }
            foreach (IObservable<List<byte>> item in e.NewItems)
            {
                var control = controlArray[index];
                var sugfn = ((queueController)sender).tmp[index++];
                item.Subscribe(new Action<List<byte>>((s) =>
                {
                    var dir = new DirectoryInfo(Value_downloader.DefaultPath);
                    try
                    {
                        System.IO.FileStream resource = new System.IO.FileStream((dir.FullName + @"\" + sugfn).Normalize(), System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, FileShare.Inheritable, 8, true);

                        int rx = s.Count;
                        control.Invoke(new Action(() => control.label2.Text = "WRITE"));
                        int counter = 0;
                        int chunkSize = 65536;
                        control.progressBar1.Value = 0;
                        control.progressBar1.Maximum = rx;
                        resource.WriteAsync(s, chunkSize).ObserveOn(control).Subscribe((x) => { try { control.progressBar1.Value += chunkSize; } catch { Debug.Print("{0}/{1}", control.progressBar1.Value, control.progressBar1.Maximum); } control.label1.Text = String.Format("{0} / {1} (バイト)", counter++ * chunkSize, rx); }, onError: (Exception ex) => System.Windows.Forms.MessageBox.Show(ex.Message), onCompleted: () => control.MarkAsCompleted());

                    }
                    catch (Exception)
                    {
                        //Todo:
                    }
                }));
            }
        }
        private void reg_event()
        {
            queue_Instance.CollectionChanged += queue_Instance_CollectionChanged;
        }

        public QueueResolver(IContainer container)
        {
            container.Add(this);
            reg_event();
            InitializeComponent();
        }

        public System.Windows.Forms.Panel NormalQueuePanel { get; set; }

        public void factory(Tuple<string, Uri, string> arg, dlQueue view)
        {
            var element = arg;
            var filenamewithExt = element.Item1;
            if (String.IsNullOrEmpty(filenamewithExt))
            {
                filenamewithExt = DateTime.Now.ToString("yyyyMMddhhmmss") + ".flv";
            }

            var data = new List<byte>();
            int latestrange = 0;

            var req = WebRequest.CreateHttp(element.Item2);
            req.Headers.Add(System.Net.HttpRequestHeader.Cookie, arg.Item3);
            view.fn.Text = filenamewithExt;

            view.label2.Text = "DL";
            var m = new kkAsync.Class2.Edd(req);
            var obs = m.myObservable
                .Do<AsynchronousExtensions.Progress<byte[]>>(onNext: new Action<AsynchronousExtensions.Progress<byte[]>>(p =>
                   {
                       view.BeginInvoke(new Action(() =>
                       {
                           view.progressBar1.Maximum = (int)p.TotalBytesToReceive;
                           view.label1.Text = String.Format("{0}MB / {1}MB", Math.Round((double)data.Count / 1000000, 3), Math.Round((latestrange + p.TotalBytesToReceive) / 1000000, 2));
                           view.progressBar1.Value = m.BytesDownloaded;
                       }));
                   }))
                   .TimeInterval()
                   .Aggregate(data, (list, p) =>
                     {

                         list.AddRange(p.Value.Value);
                         return list;
                     });
            queue_Instance.Add(obs, filenamewithExt, view);
            var c = m.Connect();
                  Observable.FromEventPattern(view.radioButton1, "CheckedChanged", new ControlScheduler(view)).Subscribe(new Action<EventPattern<object>>(s => { Logger.Push("Resume/Pause", 0); if (((System.Windows.Forms.RadioButton)s.Sender).Checked) { c.Dispose(); } else { c = m.Connect(); } }));
  }
        public async void factory(Fiddler.Session arg, proxy view)
        {
            var res = arg.oResponse;
            var sugfn = string.Empty;

            string t = arg.fullUrl;
            if (t.Contains('?') | t.Contains('@'))
            {
                t = t.Remove(t.IndexOfAny(new[] { '@', '?' }));
                t = t.Substring(t.LastIndexOf('/') + 1);
                sugfn = t;
            }
            else
            {
                sugfn = arg.SuggestedFilename;
            }
            if (String.IsNullOrWhiteSpace(sugfn) || sugfn.Contains(".txt")) { Logger.Push("Could not get filename", 3); }
            view.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            view.fn.Text = sugfn;
            view.label1.Text = @"受信終了待ち.....";

            await System.Threading.Tasks.Task.Run(() => queue_Instance.Add(Observable.Defer<List<byte>>(
                   (async () =>
                     {
                         await System.Threading.Tasks.Task.Run(() => waitforproxy(arg));
                         view.BeginInvoke(new Action(()=>view.label1.Text = "デコード...."));
                         await System.Threading.Tasks.Task.Run(() => arg.utilDecodeResponse());
                         IObservable<List<byte>> r;
                         r = arg.ResponseBody.ToObservable().Aggregate(new List<byte>(), (data, s) => { data.Add(s); return data; });
                         return r;
                     })), sugfn, view));
        }
        private int waitforproxy(Fiddler.Session session_instance)
        {
            while (session_instance.state != SessionStates.Done)
            {
                System.Threading.Thread.Sleep(500);
                if (session_instance.state == SessionStates.Aborted) { return -1; }
                Logger.Push(session_instance.state.ToString(), 0);
            }
            return 0;
        }
    }

    internal class queueController : ObservableCollection<IObservable<List<byte>>>
    {
        protected internal string[] tmp;
        protected internal View_prototype[] tmp2;
        internal void Add(IObservable<List<byte>> value , string filename, View_prototype view)
        {
            IDisposable d = new CompositeDisposable(view);
            tmp = new string[] {rslibMonoCSharp.Utils.AdjustToFileNameFormat.RemoveInvalidPathChar(filename)};
            tmp2 = new[] { view };
            this.Add(item: value);
        }
    }
}
