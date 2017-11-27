using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using ExitGames.Logging;
using System.IO;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MyGameServer.Manager;
using MyGameServer.Net;

namespace MyGameServer
{
    public enum ReturnCode
    {
        Success = 0,
        Failed = -1
    }

    // 所有的Server端 主类都要继承自applicationbase
    public class MyGameServer : ApplicationBase
    {
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static MyGameServer Instance { get; private set; }

        // 当一个客户端请求连接的时候，服务器端就会调用这个方法
        // 我们使用peerbase,表示和一个客户端的链接,然后photon就会把这些链接管理起来
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("一个客户端连接进来了！");
            Client peer = new Client(initRequest);//每链接一个客户端过来我们就把这个客户端存储起来添加到List里面
            ClientMgr.Instance.allPeerList.Add(peer);
            return peer;
        }
        //初始化(当整个服务器启动起来的时候调用这个初始化)
        protected override void Setup()
        {
            Instance = this;

            // 日志的初始化(定义配置文件log4net位置)

            // Path.Combine  表示连接目录和文件名，可以屏蔽平台的差异
            // Photon: ApplicationLogPath 就是配置文件里面路径定义的属性
            //this.ApplicationPath 表示可以获取photon的根目录,就是Photon-OnPremise-Server-SDK_v4-0-29-11263\deploy这个目录
            // 这一步是设置日志输出的文档文件的位置，这里我们把文档放在Photon-OnPremise-Server-SDK_v4-0-29-11263\deploy\bin_Win64\log里面
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(Path.Combine(Path.Combine(this.ApplicationRootPath, "bin_win64")), "log");
            //this.BinaryPath表示可以获取的部署目录就是目录Photon-OnPremise-Server-SDK_v4-0-29-11263\deploy\MyGameServer\bin
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));// 告诉log4net日志的配置文件的位置
            // 如果这个配置文件存在
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);// 设置photon我们使用哪个日志插件
                XmlConfigurator.ConfigureAndWatch(configFileInfo);// 让log4net这个插件读取配置文件
            }

            log.Info("Setup Completed!");// 最后利用log对象就可以输出了

            ServerMgr.Instance.OnServerOpen();
        }

        // server端关闭的时候
        protected override void TearDown()
        {
            ServerMgr.Instance.OnServerClose();

            log.Info("关闭了服务器");
        }


    }
}
