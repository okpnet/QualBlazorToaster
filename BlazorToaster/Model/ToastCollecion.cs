using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastCollecion<T>:IToastModelCollsection<T>,IEnumerable<IToastModel<T>>
    {
        private List<IToastModel<T>> collection = new();

        public int MaxQeueSize { get; set; }

        public int DefaultCloseTime { get; set; } = 3000;

        public Guid Enqueue(T content)
        {
            var guid= Guid.NewGuid();
            collection.Add(new ToastModel<T>(guid,()=>collection.RemoveAll(t=>t.Id==guid),content,DefaultCloseTime));
            return guid;
        }

        public Guid Enqueue(T content, int closeTime)
        {
            var guid = Guid.NewGuid();
            collection.Add(new ToastModel<T>(guid, () => collection.RemoveAll(t => t.Id == guid), content, closeTime));
            return guid;
        }

        public bool TryDequeue(out IToastModel<T> model)
        {
            if (collection.Count == 0)
            {
                model = default!;
                return false;
            }
            model= collection.First();
            return true;
        }

        public IEnumerator<IToastModel<T>> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()=>GetEnumerator();
    }
}
