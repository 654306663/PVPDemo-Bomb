using UnityEngine;
using System.Collections;
using System;
using ExitGames.Client.Photon;
using Tools;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using ProtoData;

namespace Net
{
    public class SyncPlayerHandler : HandlerBase
    {

        public override void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
        }

        public override void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
        }

        /// <summary>
        /// 收到玩家加入消息
        /// </summary>
        /// <param name="response"></param>
        void OnSyncAddPlayerReceived(OperationResponse response)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(response.Parameters, 1);
            AddPlayerS2C playerS2C = BinSerializer.DeSerialize<AddPlayerS2C>(bytes);

            BattleSyncMgr.Instance.OnSyncPlayerResponse(playerS2C.dataList);
        }
    }
}
