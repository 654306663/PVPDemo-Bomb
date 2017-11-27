using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Tools
{
    public class DictTool
    {
        //扩展方法
        public static Value GetValue<Key, Value>(Dictionary<Key, Value> dicr, Key key)
        {
            Value value;

            bool isSuccess = dicr.TryGetValue(key, out value);
            if (isSuccess)
            {
                return value;
            }
            else
            {
                return default(Value);
            }
        }
    }
}
