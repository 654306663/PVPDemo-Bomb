using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGameServer.Threads
{
    class TimerThread
    {
        static Thread t;

        static Action action;

        public static Action Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
            }
        }

        // 每帧间隔
        public static float DeltaTime()
        {
            return 1f / FrameRate();
        }

        // 帧率
        public static int FrameRate()
        {
            return 30;
        }

        // 获取服务器运行时间 毫秒
        public static int GetNowTime()
        {
            return timer * FrameRate();
        }

        //启动线程的方法
        public static void Start()
        {
            t = new Thread(Updata);// 开启线程
            t.IsBackground = true;//后台运行
            t.Start();//启动线程
        }

        static int timer = 0;
        static void Updata()
        {
            while (true)//死循环
            {
                Thread.Sleep(FrameRate());
                if (Action != null) Action();
                timer++;
            }
        }

        //关闭线程
        public static void Stop()
        {
            t.Abort();//终止线程
        }
    }
}
