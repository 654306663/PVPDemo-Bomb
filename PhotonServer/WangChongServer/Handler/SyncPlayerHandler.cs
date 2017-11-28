using MyGameServer.Net;
using MyGameServer.Tools;
using Photon.SocketServer;
using ProtoData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyGameServer.Handler
{
    class SyncPlayerHandler : IHandlerBase
    {
        public void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
            HandlerMediat.AddListener(MessageCode.RemovePlayer, OnSyncRemovePlayerReceived);
        }

        public void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
            HandlerMediat.RemoveListener(MessageCode.RemovePlayer, OnSyncRemovePlayerReceived);
        }

        //
        public void OnSyncAddPlayerReceived(Client peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            MyGameServer.log.Info("玩家进入战斗：" + peer.playerData.username);

            ClientMgr.Instance.AddBattlePeer(peer);

            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(operationRequest.Parameters, 1);
            AddPlayerC2S playerC2S = BinSerializer.DeSerialize<AddPlayerC2S>(bytes);
            peer.playerData.heroData.modelName = playerC2S.modelName;
            peer.playerData.nickname = playerC2S.nickName;
            peer.playerData.heroData.hp = playerC2S.hp;
            peer.playerData.heroData.killCount = 0;
            peer.playerData.heroData.isLife = true;

            AddPlayerS2C playerS2C = new AddPlayerS2C();
            //取得所有已经登陆（在线玩家）的用户名
            foreach (Client tempPeer in ClientMgr.Instance.BattlePeerList)
            {
                //string.IsNullOrEmpty(tempPeer.username);//如果用户名为空表示没有登陆
                //如果连接过来的客户端已经登陆了有用户名了并且这个客户端不是当前的客户端
                if (!string.IsNullOrEmpty(tempPeer.playerData.username) && tempPeer != peer)
                {
                    //把这些客户端的Usernam添加到集合里面
                    AddPlayerS2C.PlayerData playerData = new AddPlayerS2C.PlayerData();
                    playerData.username = tempPeer.playerData.username;
                    playerData.modelName = tempPeer.playerData.heroData.modelName;
                    playerData.nickName = tempPeer.playerData.nickname;
                    playerData.hp = tempPeer.playerData.heroData.hp;
                    playerData.killCount = tempPeer.playerData.heroData.killCount;
                    playerS2C.dataList.Add(playerData);
                }
            }
            byte[] bytes2 = BinSerializer.Serialize(playerS2C);
            // 告诉当前客户端其它客户端的名字
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, bytes2);
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = data;
            peer.SendOperationResponse(response, sendParameters);

            // 告诉其它客户端有新的客户端加入
            AddPlayerS2CEvt playerS2CEvt = new AddPlayerS2CEvt();
            playerS2CEvt.username = peer.playerData.username;
            playerS2CEvt.modelName = peer.playerData.heroData.modelName;
            playerS2CEvt.nickName = peer.playerData.nickname;
            playerS2CEvt.hp = peer.playerData.heroData.hp;
            playerS2CEvt.killCount = peer.playerData.heroData.killCount;
            byte[] bytes3 = BinSerializer.Serialize(playerS2CEvt);
            foreach (Client tempPeer in ClientMgr.Instance.BattlePeerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.playerData.username) && tempPeer != peer)
                {
                    EventData ed = new EventData((byte)MessageCode.AddPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add(1, bytes3);    // 把新进来的用户名传递给其它客户端
                    ed.Parameters = data2;
                    tempPeer.SendEvent(ed, sendParameters); // 发送事件
                }
            }
        }

        //
        public void OnSyncRemovePlayerReceived(Client peer, OperationRequest operationRequest, SendParameters sendParameters)
        {
            ClientMgr.Instance.RemoveBattlePeer(peer);
            MyGameServer.log.Info("玩家退出战斗：" + peer.playerData.username);

            //取得所有已经登陆（在线玩家）的用户名
            List<string> usernameList = new List<string>();
            foreach (Client tempPeer in ClientMgr.Instance.BattlePeerList)
            {
                //string.IsNullOrEmpty(tempPeer.username);//如果用户名为空表示没有登陆
                //如果连接过来的客户端已经登陆了有用户名了并且这个客户端不是当前的客户端
                if (!string.IsNullOrEmpty(tempPeer.playerData.username) && tempPeer != peer)
                {
                    //把这些客户端的Usernam添加到集合里面
                    usernameList.Add(tempPeer.playerData.username);
                }
            }

            //通过xml序列化进行数据传输,传输给客户端
            StringWriter sw = new StringWriter();
            XmlSerializer serlizer = new XmlSerializer(typeof(List<string>));
            serlizer.Serialize(sw, usernameList);
            sw.Close();
            string usernameListString = sw.ToString();

            // 告诉其它客户端有客户端离开
            foreach (Client tempPeer in ClientMgr.Instance.BattlePeerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.playerData.username) && tempPeer != peer)
                {
                    EventData ed = new EventData((byte)MessageCode.RemovePlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add(1, peer.playerData.username);    // 把新进来的用户名传递给其它客户端
                    ed.Parameters = data2;
                    tempPeer.SendEvent(ed, sendParameters); // 发送事件
                }
            }
        }
    }
}
