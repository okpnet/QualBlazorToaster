using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToastTest.Model
{
    public class ToastMessage
    {
        public Guid Id { get; set; }

        public bool IsEvent { get; set; }

        public ToastMessage()
        {
            Id=Guid.NewGuid();
        }
    }
}
