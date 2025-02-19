using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Observe
{
    public class ToastObservable<T> : IObservable<T>
    {

        List<IObserver<T>> _observers = [];

        public IDisposable Subscribe(Action<T> onNext)
        {
            var observer = new ToastObserver(onNext);
            return Subscribe(observer);
        }
        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observers.Add(observer);
            return new UnToastObserver(() => _observers.Remove(observer));
        }

        public void Run(T value)
        {
            foreach (var observer in _observers.ToList())
            {
                observer.OnNext(value);
            }
            //if (_observers.Count == 0)
            //{
            //    return;
            //}

            //var enumrator=_observers.GetEnumerator();
            //while (enumrator.MoveNext())
            //{
            //    var observerr=enumrator.Current;
            //    if(observerr is not null)
            //    {
            //        observerr.OnNext(value);
            //    }
            //}
        }
        public class ToastObserver : IObserver<T>
        {
            readonly Action<T> _onNext;

            public ToastObserver(Action<T> onNext)
            {
                _onNext = onNext;
            }

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(T value)
            {
                _onNext(value);
            }
        }
        public class UnToastObserver : IDisposable
        {
            readonly Action remove;

            public UnToastObserver(Action remove)
            {
                this.remove = remove;
            }

            public void Dispose()
            {
                remove.Invoke();
            }
        }
    }




}
