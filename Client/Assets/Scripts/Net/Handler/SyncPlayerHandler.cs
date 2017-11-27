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
            HandlerMediat.AddListener(MessageCode.AddPlayer, OnSyncPlayerReceived);
        }

        public override void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.AddPlayer, OnSyncPlayerReceived);
        }


        void OnSyncPlayerReceived(OperationResponse response)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(response.Parameters, 1);
            PlayerS2C playerS2C = BinSerializer.DeSerialize<PlayerS2C>(bytes);

            BattleSyncMgr.Instance.OnSyncPlayerResponse(playerS2C.dataList);
        }
    }
}
