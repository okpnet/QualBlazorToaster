using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Observe
{
    public class ToastObserverDiposable:IDisposable
    {
        readonly Action _dispose;

        public ToastObserverDiposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose.Invoke();
        }
    }
}
