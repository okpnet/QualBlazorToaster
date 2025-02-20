using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public enum ToastState
    {
        Stop,
        Running,
        Complete,
        Removed,
    }
}
