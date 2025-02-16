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

        Guid Enqueue(T content);

        Guid Enqueue(T content,int closeTime);

        bool TryDequeue(out IToastModel<T> model);

    }
}
