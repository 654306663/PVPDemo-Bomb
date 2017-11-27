using MyGameServer.Manager;
using MyGameServer.Threads;
using MyGameServer.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace MyGameServer.Handler
{
    class BombHandler : IHandlerBase
    {
        public void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.AddBomb, OnAddBombReceived);
        }

        public void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.AddBomb, OnAddBombReceived);
        }

        //获取客户端位置请求的处理的代码
        public void OnAddBombReceived(ClientPeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {

            //接收位置并保持起来
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(operationRequest.Parameters, 1);
            ProtoData.AddBombC2S addBombC2S = BinSerializer.DeSerialize<ProtoData.AddBombC2S>(bytes);

            ProtoData.AddBombS2CEvt addBombS2CEvt = new ProtoData.AddBombS2CEvt();
            addBombS2CEvt.username = peer.username;
            addBombS2CEvt.bombType = addBombC2S.bombType;
            addBombS2CEvt.bombId = BombMgr.Instance.GetBombId();
            addBombS2CEvt.startX = addBombC2S.startX;
            addBombS2CEvt.startY = addBombC2S.startY;
            addBombS2CEvt.startZ = addBombC2S.startZ;
            addBombS2CEvt.endX = addBombC2S.endX;
            addBombS2CEvt.endY = addBombC2S.endY;
            addBombS2CEvt.endZ = addBombC2S.endZ;

            byte[] bytes2 = BinSerializer.Serialize(addBombS2CEvt);
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.username))
                {
                    EventData ed = new EventData((byte)MessageCode.AddBomb);
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add(1, bytes2);    // 把新进来的用户名传递给其它客户端
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, sendParameters); // 发送事件
                }
            }

            BombData bombData = new BombData();
            bombData.bombId = addBombS2CEvt.bombId;
            bombData.damageRange = addBombC2S.damageRange;
            bombData.durationTime = addBombC2S.durationTime;
            bombData.endX = addBombS2CEvt.endX;
            bombData.endY = addBombS2CEvt.endY;
            bombData.endZ = addBombS2CEvt.endZ;

            ServerMgr.Instance.bombUpdate.AddBomb(bombData);
        }
    }
}
