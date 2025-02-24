using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Observe
{
    public class ToastObservable<T> : IObservable<T>
    {
        readonly ReaderWriterLockSlim _readerWriterLockSlim;

        readonly IList<IObserver<T>> _observablelist;

        public ToastObservable()
        {
            _observablelist = new List<IObserver<T>>();
            _readerWriterLockSlim = new ReaderWriterLockSlim();
        }

        public IDisposable Subscribe(Action<T> observer)
        {
            return Subscribe(new ToastObserver<T>(observer));
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            try
            {
                _readerWriterLockSlim.EnterWriteLock();
                _observablelist.Add(observer);
                return new ToastObserverDiposable(() =>Remove(observer));
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Run(T value)
        {
            try
            {
                _readerWriterLockSlim.EnterReadLock();
                foreach(var observer in _observablelist)
                {
                    observer.OnNext(value);
                }
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
        }

        private void Remove(IObserver<T> observer)
        {
            var index = _observablelist.IndexOf(observer);
            if (0 > index)
            {
                return;
            }
            _observablelist.RemoveAt(index);
        }
    }

    public static class ToastObservableExtension
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable,Action<T> onNext)
        {
            if(observable is not ToastObservable<T> tostObservable)
            {
                throw new NotImplementedException();
            }
            return tostObservable.Subscribe(onNext);
        }
    }
}
