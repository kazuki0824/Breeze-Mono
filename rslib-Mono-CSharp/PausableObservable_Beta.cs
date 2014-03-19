using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;

namespace rslibMonoCSharp
{
    class PausableObservaβle
    {
        public static class ObservableHelper
        {
            public static IConnectableObservable<TSource> WhileResumable<TSource>(Func<bool> condition, IObservable<TSource> source)
            {
                var buffer = new Queue<TSource>();
                var subscriptionsCount = 0;
                var isRunning = Disposable.Create(() =>
                {
                    lock (buffer)
                    {
                        subscriptionsCount--;
                    }
                });
                var raw = Observable.CreateWithDisposable<TSource>(subscriber =>
                {
                    lock (buffer)
                    {
                        subscriptionsCount++;
                        if (subscriptionsCount == 1)
                        {
                            while (buffer.Count > 0)
                            {
                                subscriber.OnNext(buffer.Dequeue());
                            }
                            Observable.While(() => subscriptionsCount > 0 && condition(), source)
                                .Subscribe(
                                    v => { if (subscriptionsCount == 0) buffer.Enqueue(v); else subscriber.OnNext(v); },
                                    e => subscriber.OnError(e),
                                    () => { if (subscriptionsCount > 0) subscriber.OnCompleted(); }
                                );
                        }
                    }
                    return isRunning;
                });
                return raw.Publish();
            }
        }
    }
}
