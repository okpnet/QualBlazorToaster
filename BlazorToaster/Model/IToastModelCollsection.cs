using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastModelCollsection
    {
        int MaxQeueSize { get; }

        Guid Enqueue(IToastModel toastModel);

        IToastModel Dequeue();
    }
}
