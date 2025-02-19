using BlazorToaster.Observe;
using System.Collections;

namespace BlazorToaster.Model
{
    public class ToastCollecion<T>:IToastModelCollsection<T>,IEnumerable<IToastModel<T>>
    {

        private int index;

        private List<IToastModel<T>> _collection = new();

        public ToastObservable<T> CollectionAddObservable { get; }=new();

        public int MaxQeueSize { get; set; }

        public int DefaultCloseTime { get; set; } = 3000;

        public ToastCollecion()
        {
        }

        public void Enqueue(T content)
        {
            var guid= Guid.NewGuid();
            var addItem = new ToastModel<T>(guid, () => _collection.RemoveAll(t => t.Id == guid), content, DefaultCloseTime);
            addItem.StateChangeObservable.Subscribe((a) => CollectionAddObservable.Run(a));
            _collection.Add(addItem);
            CollectionAddObservable.Run(content);
        }

        public void Enqueue(T content, int closeTime)
        {
            var guid = Guid.NewGuid();
            var addItem = new ToastModel<T>(guid, () => _collection.RemoveAll(t => t.Id == guid), content, closeTime);
            addItem.StateChangeObservable.Subscribe((a) => CollectionAddObservable.Run(a));
            _collection.Add(addItem);
            CollectionAddObservable.Run(content);
        }

        public bool TryDequeue(out IToastModel<T> model)
        {
            if (_collection.Count == 0 || index >= _collection.Count)
            {
                model = default!;
                index = 0;
                return false;
            }
            try
            {
                model = _collection[index];
                index += 1;
                return true;
            }
            catch (Exception ex)
            {
                model = default!;
                return false;
            }
        }

        public void Cancel(T content)
        {
            _collection.FirstOrDefault(t => Equals(t.Content, content))?.Cancel();
        }

        public void Close(T content)
        {
            _collection.FirstOrDefault(t => Equals(t.Content, content))?.Close();
        }

        public IEnumerator<IToastModel<T>> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()=>GetEnumerator();
    }
}
