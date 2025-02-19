using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastModelCollsection<T>
    {
        ToastConfigure Configure { get; }

        IEnumerable<IToastModel<T>> EnableToasts { get; }

        IObservable<T> ChangeCollecitonObservable { get; }

        void Enqueue(T content);

        void Enqueue(T content,int closeTime);

        void Cancel(T content);

        void Close(T content);

        void Remove(IToastModel<T> model);
    }
}
