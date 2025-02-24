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

        void Enqueue(T content);

        void Enqueue(T content, IToastConfigure configure);

        void Cancel();

        void Cancel(T content);

        Task CloseAsync(T content);

        void Remove(IToastModel<T> model);

        void RemoveAll();
    }
}
