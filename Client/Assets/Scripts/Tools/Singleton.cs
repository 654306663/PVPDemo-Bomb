public class Singleton<T> where T : new()
{
    private static readonly object sycObj = new object();
    private static T t;
    public static T Instance
    {
        get
        {
            if (t == null)
            {
                lock (sycObj)
                {
                    if (t == null)
                    {
                        t = new T();
                    }
                }
            }
            return t;
        }
    }
}