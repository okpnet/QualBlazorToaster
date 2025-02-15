using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorToaster.Model
{
    public interface IToastModel
    {
        Guid Id { get; }
        int CloosedTimer { get; }

        string Message { get; }

        bool CloseButton { get; }

    }
}
