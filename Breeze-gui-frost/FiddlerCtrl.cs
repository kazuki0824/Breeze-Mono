using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Breeze_gui_frost
{
	public partial class FiddlerCtrl : Component
	{
		public FiddlerCtrl()
		{
			InitializeComponent();
		}

		public FiddlerCtrl(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}
		bool isRunning;
		public void startFiddler()
		{
			if (isRunning) {return;}
			Fiddler.FiddlerApplication.ResponseHeadersAvailable += FiddlerApplication_ResponseHeaderAvailable;
			Fiddler.CONFIG.IgnoreServerCertErrors = true;
			
			Fiddler.CONFIG.bStreamAudioVideo = true;
			Fiddler.FiddlerApplication.Startup(0, Fiddler.FiddlerCoreStartupFlags.CaptureLocalhostTraffic);
			//TODO: クロスプラットフォーム恐らく未対応
			Fiddler.URLMonInterop.SetProxyInProcess(string.Format("127.0.0.1:{0}", Fiddler.FiddlerApplication.oProxy.ListenPort), "<local>");
			isRunning = true;
		}
		private void shutdown()
		{
			Fiddler.URLMonInterop.ResetProxyInProcessToDefault();
			Fiddler.FiddlerApplication.Shutdown();
		}

		public event FiddlerCaughtVideoEventHandler FiddlerCaughtNormalMedia;
		public delegate void FiddlerCaughtVideoEventHandler( Fiddler.Session oSession);
		private void FiddlerApplication_ResponseHeaderAvailable(Fiddler.Session oSession)
		{
#if DEBUG
			Logger.Push(String.Format("{0}:HTTP {1} for {2}", oSession.id, oSession.responseCode, oSession.fullUrl),0);
#endif
			Parallel.ForEach<string>(Value_downloader.VideoMimelist, (string pattern) => { if (rslib_StringEvaluator.Evaltool.Evaluation(oSession.oResponse.MIMEType, pattern)) { FiddlerCaughtNormalMedia(oSession); } });
		}
		public void Close()
		{
			shutdown();
			Dispose();
		}
	}
}
