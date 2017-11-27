using Photon.SocketServer;
using ProtoData;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyGameServer.Threads
{
    class SyncTransformUpdate
    {
        public void Updata()
        {
            //进行同步
            SendPosition();
        }
        
        //把所有客户端的位置信息发送到各个客户端
        //封装位置信息，封装到字典里,然后利用Xml序列化去发送
        private void SendPosition()
        {
            if (MyGameServer.Instance.peerList.Count == 0) return;

            SyncTransformEvtS2C syncPositionEvtS2C = new SyncTransformEvtS2C();

            //装载PlayerData里面的信息
            foreach (ClientPeer peer in MyGameServer.Instance.peerList)//遍历所有客户段
            {
                if (string.IsNullOrEmpty(peer.username) == false)//取得当前已经登陆的客户端
                {
                    SyncTransformEvtS2C.TransformData transformData = new SyncTransformEvtS2C.TransformData();
                    transformData.username = peer.username;//设置playerdata里面的username
                    transformData.x = peer.x;//设置playerdata里面的Position
                    transformData.y = peer.y;
                    transformData.z = peer.z;
                    transformData.angleY = peer.angleY;
                    syncPositionEvtS2C.dataList.Add(transformData);//把playerdata放入集合
                }
            }
            byte[] bytes = Tools.BinSerializer.Serialize(syncPositionEvtS2C);

            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, bytes);//把所有的playerDataListString装载进字典里面
            EventData ed = new EventData((byte)MessageCode.SyncTransform);
            ed.Parameters = data;
            //把信息装在字典里发送给各个客户端
            foreach (ClientPeer peer in MyGameServer.Instance.peerList)
            {
                if (string.IsNullOrEmpty(peer.username) == false)
                {
                    peer.SendEvent(ed, new SendParameters());
                }
            }
        }
    }
}
