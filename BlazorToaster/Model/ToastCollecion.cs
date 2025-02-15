using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public class ToastCollecion:IToastModelCollsection
    {
        private List<IToastModel> collection = new();

        private  int index=0;


    }
}
