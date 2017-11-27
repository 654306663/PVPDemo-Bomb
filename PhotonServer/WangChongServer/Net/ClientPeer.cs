using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServer.Handler;

namespace MyGameServer
{
    //管理跟客户端的链接的
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public string username;

        public float x, y, z;

        public float angleY;

        public string modelName;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }
        //处理客户端断开连接的后续工作
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyGameServer.log.Info("一个客户端失去连接" + username);
            MyGameServer.Instance.peerList.Remove(this);    // 断开连接的时候List里面移除当前的ClientPeer客户端

            // 告诉其它客户端有客户端离开
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.username))
                {
                    EventData ed = new EventData((byte)MessageCode.RemovePlayer);
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add(1, username);    // 把新进来的用户名传递给其它客户端
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, new SendParameters()); // 发送事件
                }
            }
        }
        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //OperationRequest封装了请求的信息
            //SendParameters 参数，传递的数据

            HandlerMediat.Dispatch((MessageCode)operationRequest.OperationCode, this, operationRequest, sendParameters);  // 分发消息
        }
    }
}
