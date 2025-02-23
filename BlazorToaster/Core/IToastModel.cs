using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Core
{
    public interface IToastModel<T> : IDisposable
    {
        Guid Id { get; }

        IToastConfigure Configure { get; }

        T Content { get; }

        ToastState State { get; }

        CancellationToken CancelToken { get; }

        IObservable<T> ChangeObservable { get; }

        void Cancel();

        Task CloseAsync();

        Task StartAsync();
    }
}
