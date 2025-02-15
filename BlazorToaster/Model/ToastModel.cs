using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastModel:IToastModel
    {
        public Guid Id { get; }=Guid.NewGuid();

        public int CloosedTimer { get; } = 3000;

        public string Message { get; set; } = string.Empty;

        public bool CloseButton { get; set; }

        public ToastModel(int cloosedTimer)
        {
            CloosedTimer = cloosedTimer;
        }
    }
}
