using BlazorToaster.Observe;
using System.Collections;
using System.Reflection;

namespace BlazorToaster.Model
{
    public class ToastCollecion<T>:IToastModelCollsection<T>,IDisposable
    {
        readonly private ReaderWriterLockSlim _readerWriterLockSlim;

        readonly private ToastObservable<T> _toastObservable;

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
                    return _collection.Where(t=>t.State!=ToastState.Removed).Take(Configure.MaxToast);
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
            Configure = configure;
        }

        public void Enqueue(T content)=>Enqueue(content,Configure.Duration);

        public void Enqueue(T content, int closeTime)
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
            Add(new ToastModel<T>(guid, removeAction, content, closeTime));
        }
        

        public void Cancel(T content)
        {
            _collection.FirstOrDefault(t => Equals(t.Content, content))?.Cancel();
        }

        public void Close(T content)
        {
            _collection.FirstOrDefault(t => Equals(t.Content, content))?.Close();
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
                var index=_collection.IndexOf(model);
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
            _readerWriterLockSlim.Dispose();
        }

        protected void RemoveAll()
        {
            try
            {
                _readerWriterLockSlim.EnterWriteLock();
                foreach (var model in _collection)
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
