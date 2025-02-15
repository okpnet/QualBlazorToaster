using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastModel:IToastModel,IDisposable
    {
        readonly Action _removeAction;
        public Guid Id { get; }=Guid.NewGuid();

        public int CloosedTimer { get; } = 3000;

        public string Message { get; set; } = string.Empty;

        public bool CloseButton { get; set; }


        public ToastModel(Action removeAction,int cloosedTimer)
        {
            CloosedTimer = cloosedTimer;
            _removeAction = removeAction;
        }

        public async Task SaartAsync()
        {
            if (0 > CloosedTimer)
            {
                return;
            }
            await Task.Delay(CloosedTimer).ContinueWith(task => 
            {
                if (task.Status == TaskStatus.Canceled)
                {
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
