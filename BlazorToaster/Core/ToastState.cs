using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Core
{
    public enum ToastState:int
    {
        Stanby=0x0,
        Start=0x1,
        Run=0x2,
        Stop=0x3,
        Delete=0x4,
    }
}
