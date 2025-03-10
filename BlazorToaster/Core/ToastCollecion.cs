﻿using BlazorToaster.Observe;
using System.Collections;
using System.Reflection;

namespace BlazorToaster.Core
{
    public class ToastCollecion<T> : IToastModelCollsection<T>, IDisposable
    {
        readonly private ReaderWriterLockSlim _readerWriterLockSlim;

        readonly private ToastObservable<T> _toastObservable;

        readonly private IList<IDisposable> _disposables;   

        private List<IToastModel<T>> _collection = new();

        public ToastConfigure Configure { get; set; }

        public IObservable<T> ChangeCollecitonObservable => _toastObservable;

        public IEnumerable<IToastModel<T>> EnableToasts
        {
            get
            {
                try
                {
                    _readerWriterLockSlim.EnterReadLock();
                    return _collection.Where(t => t.State != ToastState.Delete).Take(Configure.MaxToast);
                }
                finally
                {
                    _readerWriterLockSlim.ExitReadLock();
                }
            }
        }

        public ToastCollecion(ToastConfigure configure)
        {
            _readerWriterLockSlim = new ReaderWriterLockSlim();
            _toastObservable = new ToastObservable<T>();
            _disposables = [];
            Configure = configure;
        }

        public void Enqueue(T content) => Enqueue(content, Configure);

        public void Enqueue(T content, IToastConfigure configure)
        {
            var guid = Guid.NewGuid();
            var removeAction = () =>
            {
                var result = _collection.FirstOrDefault(t => t.Id == guid);
                if (result is null)
                {
                    return;
                }
                Remove(result);
            };
            var addModel = new ToastModel<T>(guid, removeAction, content, configure);
            _disposables.Add(addModel.ChangeObservable.Subscribe(_ => _toastObservable.Run(default!)));
            Add(addModel);
        }

        public void Cancel()
        {
            if (_collection.Count == 0)
            {
                return;
            }
            try
            {
                _readerWriterLockSlim.EnterReadLock();
                foreach(var toastModel in _collection.Where(t=> (int)t.State>(int)ToastState.Stanby && (int)ToastState.Stop>(int)t.State))
                {
                    toastModel.Cancel();
                }
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Cancel(T content)
        {
            if (_collection.Count == 0)
            {
                return;
            }
            try
            {
                _readerWriterLockSlim.EnterReadLock();
                _collection.FirstOrDefault(t => Equals(t.Content, content))?.Cancel();
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }  
        }

        public async Task CloseAsync(T content)
        {
            await (_collection.FirstOrDefault(t => Equals(t.Content, content))?.CloseAsync()??Task.CompletedTask);
        }

        public void Remove(IToastModel<T> model)
        {
            if (_collection.Count == 0)
            {
                return;
            }
            try
            {
                _readerWriterLockSlim.EnterWriteLock();
                var index = _collection.IndexOf(model);
                if(index == -1)
                {
                    return;
                }
                _collection.RemoveAt(index);
            }
            finally
            {
                _toastObservable.Run(model.Content);
                _readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            RemoveAll();
            foreach (var toastDisposable in _disposables)
            {
                toastDisposable.Dispose();
            }
            _readerWriterLockSlim.Dispose();
        }

        public void RemoveAll()
        {
            try
            {
                _readerWriterLockSlim.EnterWriteLock();
                foreach (var model in _collection.Where(t=>t.State==ToastState.Run || t.State==ToastState.Stanby))
                {
                    model.Dispose();
                }
                _collection.Clear();
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
                _toastObservable.Run(default!);
            }
        }

        protected void Add(ToastModel<T> model)
        {
            try
            {
                _readerWriterLockSlim.EnterWriteLock();
                _collection.Add(model);
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }

        }
    }
}
