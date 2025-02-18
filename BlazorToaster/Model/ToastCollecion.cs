using System.Collections;

namespace BlazorToaster.Model
{
    public class ToastCollecion<T>:IToastModelCollsection<T>,IEnumerable<IToastModel<T>>
    {
        private List<IToastModel<T>> _collection = new();

        public int MaxQeueSize { get; set; }

        public int DefaultCloseTime { get; set; } = 3000;

        public void Enqueue(T content)
        {
            var guid= Guid.NewGuid();
            _collection.Add(new ToastModel<T>(guid,()=>_collection.RemoveAll(t=>t.Id==guid),content,DefaultCloseTime));
        }

        public void Enqueue(T content, int closeTime)
        {
            var guid = Guid.NewGuid();
            _collection.Add(new ToastModel<T>(guid, () => _collection.RemoveAll(t => t.Id == guid), content, closeTime));
        }

        public bool TryDequeue(out IToastModel<T> model)
        {
            if (_collection.Count == 0)
            {
                model = default!;
                return false;
            }
            model= _collection.First();
            return true;
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
