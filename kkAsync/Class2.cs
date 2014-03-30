using System;
using System.Net;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AsynchronousExtensions;

namespace kkAsync
{
	public class Class2
	{
		public class Edd: IObservationHub<HttpWebRequest>
		{
			internal ISubject<AsynchronousExtensions.Progress<byte[]>>  x; //yを見張る、購読可能
			internal IObserver<AsynchronousExtensions.Progress<byte[]>> w;　//yに渡す分 と 
			internal HttpWebRequest i;
			internal IObservable<AsynchronousExtensions.Progress<byte[]>> y; //HttpWebRequestの分
			public bool isRunning = false;
			public Edd(HttpWebRequest req)
			{
				i = req;
				w = Observer.Create<AsynchronousExtensions.Progress<byte[]>>(s => { x.OnNext(s); byteCount += s.Value.Length; System.Diagnostics.Debug.Assert(byteCount == s.BytesReceived); }, (ex) => x.OnError(ex), () => x.OnCompleted()); //そのまますべて転送するObserverを定義
				x = new Subject<AsynchronousExtensions.Progress<byte[]>>();
				observableInstance = x.AsObservable(); 
				
			}
			//public void Dispose()
			//{
			//    x.OnError(new OperationCanceledException("ユーザーキャンセル。"));
			//    this.Dispose(true);
			//}
			private IObservable<AsynchronousExtensions.Progress<byte[]>> observableInstance; //使う側が読むやつ

			public IObservable<AsynchronousExtensions.Progress<byte[]>> myObservable
			{
				get { return observableInstance; }
			}

			public IDisposable Connect()
			{
				i = i.CloneRequest(newUri:i.RequestUri);
				i.AddRange(byteCount);
				y= i.DownloadDataAsyncWithProgress();
				var c = y.Subscribe(w);
				return Disposable.Create(() => { c.Dispose(); y = null; });
			}
			private int byteCount;

			public int BytesDownloaded
			{
				get { return byteCount; }
			}
			
		}
	}
	public abstract class IObservationHub<T>
	{
		public bool isRunning;
		public Action<T> conn { get; set; }
		public Action<T> disconn { get; set; }
	}
}
