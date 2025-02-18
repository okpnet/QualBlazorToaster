using BlazorToaster.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Extension
{
    public static class ToastExtension
    {
        public static class Factory
        {
            public static IToastModelCollsection<T> Create<T>(int activeTime)
            {
                return new ToastCollecion<T>() { DefaultCloseTime=activeTime};
            }
        }
    }
}
