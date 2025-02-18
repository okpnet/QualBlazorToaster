using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastModelCollsection<T>:IEnumerable<IToastModel<T>>
    {
        int MaxQeueSize { get; }

        int DefaultCloseTime { get; }

        void Enqueue(T content);

        void Enqueue(T content,int closeTime);

        bool TryDequeue(out IToastModel<T> model);

        void Cancel(T content);

        void Close(T content);
    }
}
