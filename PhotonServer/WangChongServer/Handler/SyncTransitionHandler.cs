using MyGameServer.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace MyGameServer.Handler
{
    class SyncTransitionHandler : IHandlerBase
    {
        public void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.SyncTransition, OnSyncTransitionReceived);
        }

        public void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.SyncTransition, OnSyncTransitionReceived);
        }

        //获取客户端位置请求的处理的代码
        public void OnSyncTransitionReceived(ClientPeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {

            //接收位置并保持起来
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(operationRequest.Parameters, 1);
            object[] objs = (object[])DictTool.GetValue<byte, object>(operationRequest.Parameters, 2);
            ProtoData.SyncTransitionC2S syncTransitionC2S = BinSerializer.DeSerialize<ProtoData.SyncTransitionC2S>(bytes);

            ProtoData.SyncTransitionS2C syncTransitionS2C = new ProtoData.SyncTransitionS2C();
            syncTransitionS2C.username = peer.username;
            syncTransitionS2C.targetTransition = syncTransitionC2S.targetTransition;
            byte[] bytes2 = BinSerializer.Serialize(syncTransitionS2C);

            // 告诉其它客户端 当前客户端改变的动作
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.username) && tempPeer != peer)
                {
                    EventData ed = new EventData((byte)MessageCode.SyncTransition);
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add(1, bytes2);  
                    data.Add(2, objs);    
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, sendParameters); // 发送事件
                }
            }
        }
    }
}
