﻿using BlazorToaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster
{
    public static class ToastExtension
    {
        public static class Factory
        {
            public static IToastModelCollsection<T> Create<T>()
            {
                return new ToastCollecion<T>(new ToastConfigure());
            }

            public static IToastModelCollsection<T> Create<T>(ToastConfigure configure)
            {
                return new ToastCollecion<T>(configure);
            }
        }
    }
}
