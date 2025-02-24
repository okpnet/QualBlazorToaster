namespace BlazorToaster.Core
{
    public static class ToastExtension
    {
        public static class Factory
        {
            public static IToastModelCollsection<T> CreateCollecion<T>()
            {
                return new ToastCollecion<T>(new ToastConfigure());
            }

            public static IToastModelCollsection<T> CreateCollecion<T>(ToastConfigure configure)
            {
                return new ToastCollecion<T>(configure);
            }
        }
    }
}
