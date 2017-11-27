using MyGameServer.Tools;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGameServer.Threads
{
    class BombData
    {
        public int bombId;
        public float durationTime;
        public float damageRange;
        public float endX, endY, endZ;

        public int startTime;
    }

    class BombUpdate
    {
        List<BombData> bombDataList = new List<BombData>();
        
        //启动线程的方法
        public void AddBomb(BombData bombData)
        {
            bombData.startTime = TimerThread.GetNowTime();
            bombDataList.Add(bombData);
        }

        void RemoveBomb(BombData bombData)
        {
            if(bombDataList.Contains(bombData))
            {
                bombDataList.Remove(bombData);
            }
        }
        
        public void Update()
        {
            for (int i = 0; i < bombDataList.Count; i++)
            {
                if (TimerThread.GetNowTime() - bombDataList[i].startTime >= bombDataList[i].durationTime * 1000)
                {
                    SendOpenBomb(bombDataList[i]);
                    RemoveBomb(bombDataList[i]);
                    i--;
                }
            }
        }        

        void SendOpenBomb(BombData bombData)
        {
            ProtoData.OpemBombS2CEvt openBombS2CEvt = new ProtoData.OpemBombS2CEvt();
            openBombS2CEvt.bombId = bombData.bombId;
            openBombS2CEvt.openType = ProtoData.OpemBombS2CEvt.OpenBombType.Normal;
            CalculateDamage(bombData, openBombS2CEvt);
            byte[] bytes = BinSerializer.Serialize(openBombS2CEvt);
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.username))
                {
                    EventData ed = new EventData((byte)MessageCode.OpenBomb);
                    Dictionary<byte, object> data = new Dictionary<byte, object>();
                    data.Add(1, bytes);    // 把新进来的用户名传递给其它客户端
                    ed.Parameters = data;
                    tempPeer.SendEvent(ed, new SendParameters()); // 发送事件
                }
            }
        }

        void CalculateDamage(BombData bombData, ProtoData.OpemBombS2CEvt openBombS2CEvt)
        {
            //装载PlayerData里面的信息
            foreach (ClientPeer peer in MyGameServer.Instance.peerList)//遍历所有客户段
            {
                if (string.IsNullOrEmpty(peer.username) == false)//取得当前已经登陆的客户端
                {
                    float lenght = (float)Math.Sqrt(Math.Pow(peer.x - bombData.endX, 2.0) + Math.Pow(peer.y - bombData.endY, 2.0) + Math.Pow(peer.z - bombData.endZ, 2.0));
                    if(lenght < bombData.damageRange)
                    {
                        ProtoData.OpemBombS2CEvt.BeHitData beHitData = new ProtoData.OpemBombS2CEvt.BeHitData();
                        beHitData.username = peer.username;
                        beHitData.lossHp = (int)((bombData.damageRange - lenght) * 10);
                        openBombS2CEvt.beHitData.Add(beHitData);
                    }
                }
            }
        }
    }
}
