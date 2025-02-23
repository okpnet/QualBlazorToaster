using System.ComponentModel.DataAnnotations;

namespace BlazorToaster.Core
{
    public class ToastConfigure: IToastConfigure
    {
        [Range(1, ToasterDefine.MAX_NUM_OF_TOASTS)]
        public int MaxToast { get; set; } = ToasterDefine.DEFAULT_NUM_OF_TOASTS;

        [Range(ToasterDefine.MIN_DURATION, ToasterDefine.MAX_DURATION)]
        public int Duration { get; set; } = ToasterDefine.DEFAULT_DURAION;
    }
}
