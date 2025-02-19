using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster
{
    public class ToastConfigure
    {
        public int MaxToast { get; set; } = 5;

        public int Duration { get; set; } = 3000;
    }
}
