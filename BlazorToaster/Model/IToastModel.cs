using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastModel<T>
    {
        Guid Id { get; }
        int ClosedTime { get; }

        T Content { get; }

        ToastState State { get; }
        
        Task StartAsync();
    }
}
