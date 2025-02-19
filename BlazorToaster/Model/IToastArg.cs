using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastArg<T>
    {

        T Content { get; }

        EventCallback CloseEvent { get; }

        ToastState State { get; }
    }
}
