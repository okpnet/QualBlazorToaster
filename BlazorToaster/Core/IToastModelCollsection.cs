using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Core
{
    public interface IToastModelCollsection<T>
    {
        ToastConfigure Configure { get; }

        IEnumerable<IToastModel<T>> EnableToasts { get; }

        IObservable<T> ChangeCollecitonObservable { get; }

        IToastArg<T> Enqueue(T content);

        IToastArg<T> Enqueue(T content, int closeTime);

        void Cancel(T content);

        Task CloseAsync(T content);

        void Remove(IToastModel<T> model);
    }
}
