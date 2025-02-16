using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastModel<T>:IToastModel<T>,IDisposable
    {
        readonly Action _removeAction;

        ToastState _state =ToastState.Stop;

        public Guid Id { get; }=Guid.NewGuid();

        public int ClosedTime { get; } = 3000;

        public T Content { get; set; } = default!;

        public ToastState State => _state;

        public ToastModel(Guid id,Action removeAction,T content,int cloosedTimer)
        {
            Id = id;
            ClosedTime = cloosedTimer;
            _removeAction = removeAction;
        }

        public async Task StartAsync()
        {
            if (0 > ClosedTime)
            {
                return;
            }
            _state = ToastState.Running;  
            await Task.Delay(ClosedTime).ContinueWith(task => 
            {
                if (task.Status == TaskStatus.Canceled)
                {
                    _state = ToastState.Complete;
                    Dispose();
                }
            });
        }

        public void Dispose()
        {
            _removeAction.Invoke();
        }
    }
}
